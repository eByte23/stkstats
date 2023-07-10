using System.Linq;
using GameChanger.Parser;
using Newtonsoft.Json;
using StatSys.CoreStats.Models;

namespace StatSys.CoreStats.Mappers;

public class ToGameMapper
{
    private UniqueIdGenerator _idGenerator = new UniqueIdGenerator();
    private readonly DeterministicGuid clubId;
    //
    public ToGameMapper()
    {
        clubId = _idGenerator.NewDeterministicId("STKBC");
    }


    public static string GetShortId(Guid id, params string[] parts)
    {
        var last12 = new string(id.ToString().TakeLast(12).ToArray());

        return string.Join("-", parts.Select(p => p.Replace("  ", " ").Replace(" ", "-")).Append(last12)).Replace("`", "").Replace("'", "").ToLower();
    }

    public GameData Map(
        string teamId,
        string gameId,
        string gameAbsoluteUrl,
        GameChanger.Parser.GameChangerApiStats.Game stats,
        GameChanger.Parser.GameChangerTeamSchedule teamSchedule)
    {
        if (gameId == "6496d2a47c0001710c00071f")
        {
            Console.WriteLine("Found it");
        }


        var (seasonId, seasonName) = GetSeasonInfoFromTeamSchedule(teamSchedule);
        var gameDate = GetGameDateFromStats(gameId, teamSchedule);
        var (teamOfInterest, oppositionTeam) = GetTeamsFromStats(stats, teamId);
        var players = GetTeamPlayersFromStats(stats, teamOfInterest);

        Guid gameUniqueId = _idGenerator.NewDeterministicId(gameId).Id;
        return new GameData
        {
            GameShortId = GetShortId(gameUniqueId, teamOfInterest.TeamName, oppositionTeam.TeamName, gameDate.ToString("yyyy-MM-dd")),
            GameUrl = gameAbsoluteUrl,
            GameId = gameUniqueId,
            GameChangerGameId = gameId,
            TeamUnqiueId = teamOfInterest.UnqiueTeamId,
            TeamShortId = teamOfInterest.TeamShortId,
            TeamId = teamOfInterest.GameChangerTeamId,
            TeamName = teamOfInterest.TeamName,
            HomeAway = teamOfInterest.IsHome ? "Home" : "Away",
            HomeRuns = teamOfInterest.IsHome ? teamOfInterest.Runs : oppositionTeam.Runs,
            AwayRuns = !teamOfInterest.IsHome ? teamOfInterest.Runs : oppositionTeam.Runs,
            Result = teamOfInterest.Result,
            SeasonId = seasonId,
            SeasonName = seasonName,
            Date = gameDate.ToString("yyyy-MM-dd"),
            Grade = "UNKNOWN",
            Location = "UNKNOWN",
            OppositionId = oppositionTeam.GameChangerTeamId,
            OppositionName = oppositionTeam.TeamName,
            Players = players,
        };
    }

    private DateTime GetGameDateFromStats(string gameId, GameChangerTeamSchedule teamSchedule)
    {
        var game = teamSchedule.TeamEvents.Single(x => x.GameId == gameId);

        return game.LocalStartDateTime.Date;
    }

    private List<PlayerData> GetTeamPlayersFromStats(GameChangerApiStats.Game stats, TeamGameInfo teamOfInterest)
    {
        var players = new List<PlayerData>();
        var team = stats.Teams.Single(x => x.Id == teamOfInterest.GameChangerTeamId);

        foreach (var player in team.Players)
        {
            string fullName = $"{player.Fname} {player.Lname}";

            var uniqueGuid = clubId.NewGuid(fullName.ToLower()).Id;
            var shortId = GetShortId(uniqueGuid, player.Fname.ToLower(), player.Lname.ToLower());

            PlayerData newPlayer = new PlayerData
            {
                ShortId = shortId,
                UniqueId = uniqueGuid,
                GameChangerIds = new List<string> { player.PlayerId },
                Name = fullName,
                Hitting = new HittingData
                {
                    PA = player.Stats.Offense.PA.ToString(),
                    AB = player.Stats.Offense.AB.ToString(),
                    H = player.Stats.Offense.H.ToString(),
                    TB = player.Stats.Offense.TB.ToString(),
                    _1B = player.Stats.Offense._1B.ToString(),
                    _2B = player.Stats.Offense._2B.ToString(),
                    _3B = player.Stats.Offense._3B.ToString(),
                    HR = player.Stats.Offense.HR.ToString(),
                    RBI = player.Stats.Offense.RBI.ToString(),
                    R = player.Stats.Offense.R.ToString(),
                    BB = player.Stats.Offense.BB.ToString(),
                    SO = player.Stats.Offense.SO.ToString(),
                    KL = player.Stats.Offense.SOL.ToString(),
                    AVG = player.Stats.Offense.AVG.ToString(),
                    SLG = player.Stats.Offense.SLG.ToString(),
                    OPS = player.Stats.Offense.OPS.ToString(),
                    OBP = player.Stats.Offense.OBP.ToString(),
                }
            };

            if (players.Any(x => x.UniqueId == uniqueGuid))
            {
                var existingPlayer = players.Single(x => x.UniqueId == uniqueGuid);

                if (existingPlayer.Hitting.PA == "0")
                {
                    players.Remove(existingPlayer);
                }
                else
                {
                    existingPlayer.GameChangerIds.Add(newPlayer.GameChangerIds.Single());
                    continue;
                }
            }



            players.Add(newPlayer);
        }

        return players.Where(x => x.Hitting.PA != "0").ToList();
    }

    private (TeamGameInfo teamOfInterest, TeamGameInfo oppositionTeam) GetTeamsFromStats(GameChangerApiStats.Game stats, string teamId)
    {
        var teamOfInterest = stats.Teams.First(x => x.Id == teamId);
        var oppositionTeam = stats.Teams.First(x => x.Id != teamId);

        Guid teamOfInterestUid = clubId.NewGuid(teamOfInterest.LongName).Id;
        Guid oppositionTeamUid = clubId.NewGuid(oppositionTeam.LongName).Id;

        var isDraw = (teamOfInterest.Stats.Offense.R == oppositionTeam.Stats.Offense.R);
        var teamOfInterestInfo = new TeamGameInfo
        {
            UnqiueTeamId = teamOfInterestUid,
            TeamShortId = GetShortId(teamOfInterestUid, teamOfInterest.LongName),
            GameChangerTeamId = teamOfInterest.Id,
            TeamName = teamOfInterest.LongName.Replace("  ", " "),
            IsHome = teamOfInterest.IsHome,
            Runs = teamOfInterest.Stats.Offense.R,
            Result = isDraw ? "D" : (teamOfInterest.Stats.Offense.R > oppositionTeam.Stats.Offense.R ? "W" : "L")
        };

        var oppositionTeamInfo = new TeamGameInfo
        {
            UnqiueTeamId = oppositionTeamUid,
            TeamShortId = GetShortId(oppositionTeamUid, oppositionTeam.LongName),
            GameChangerTeamId = oppositionTeam.Id,
            TeamName = oppositionTeam.LongName.Replace("  ", " "),
            IsHome = oppositionTeam.IsHome,
            Runs = oppositionTeam.Stats.Offense.R,
            Result = isDraw ? "D" : (teamOfInterest.Stats.Offense.R < oppositionTeam.Stats.Offense.R ? "W" : "L")
        };

        return (teamOfInterestInfo, oppositionTeamInfo);
    }

    private (string? seasonId, string? seasonName) GetSeasonInfoFromTeamSchedule(GameChangerTeamSchedule teamSchedule)
    {
        return (teamSchedule.Team.SeasonId, teamSchedule.Team.SeasonName);
    }

    public class TeamGameInfo
    {
        public Guid UnqiueTeamId { get; set; }
        public string? TeamShortId { get; set; }
        public string? GameChangerTeamId { get; set; }
        public string? TeamName { get; set; }

        public int Runs { get; set; }

        public bool IsHome { get; set; }
        public string Result { get; internal set; }
    }
}


