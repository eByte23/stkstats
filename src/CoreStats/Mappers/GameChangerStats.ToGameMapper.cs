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


    public GameData Map(
        string teamId,
        string gameId,
        string gameAbsoluteUrl,
        GameChanger.Parser.GameChangerApiStats.Game stats,
        GameChanger.Parser.GameChangerTeamSchedule teamSchedule)
    {

        var (seasonId, seasonName) = GetSeasonInfoFromTeamSchedule(teamSchedule);
        var gameDate = GetGameDateFromStats(gameId, teamSchedule);
        var (teamOfInterest, oppositionTeam) = GetTeamsFromStats(stats, teamId);
        var players = GetTeamPlayersFromStats(stats, teamOfInterest);

        Guid gameUniqueId = _idGenerator.NewDeterministicId(gameId).Id;
        return new GameData
        {
            GameShortId = StringUtils.GetShortId(gameUniqueId, teamOfInterest.TeamName, oppositionTeam.TeamName, gameDate.ToString("yyyy-MM-dd")),
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

            var firstName = StringUtils.TrimName(player.Fname);
            var lastName = StringUtils.TrimName(player.Lname);
            string fullName = StringUtils.BuildName(firstName, lastName);

            var uniqueGuid = clubId.NewGuid(fullName.ToLower()).Id;
            var shortId = StringUtils.GetShortId(uniqueGuid, fullName.ToLower());

            PlayerData newPlayer = new PlayerData
            {
                ShortId = shortId,
                PlayerId = uniqueGuid,
                GameChangerIds = new List<string> { player.PlayerId },
                Name = fullName,
                FirstName = firstName,
                LastName = lastName,
                Hitting = new HittingData
                {
                    PA = player.Stats.Offense.PA,
                    AB = player.Stats.Offense.AB,
                    H = player.Stats.Offense.H,
                    TB = player.Stats.Offense.TB,
                    Singles = player.Stats.Offense._1B,
                    Doubles = player.Stats.Offense._2B,
                    Triples = player.Stats.Offense._3B,
                    HR = player.Stats.Offense.HR,
                    RBI = player.Stats.Offense.RBI,
                    R = player.Stats.Offense.R,
                    BB = player.Stats.Offense.BB,
                    SB = player.Stats.Offense.SB,
                    SO = player.Stats.Offense.SO,
                    SF = player.Stats.Offense.SHF,
                    KL = player.Stats.Offense.SOL,
                    HBP = player.Stats.Offense.HBP,
                    AVG = player.Stats.Offense.AVG.ToString(),
                    SLG = player.Stats.Offense.SLG.ToString(),
                    OPS = player.Stats.Offense.OPS.ToString(),
                    OBP = player.Stats.Offense.OBP.ToString(),
                }
            };

            if (players.Any(x => x.PlayerId == uniqueGuid))
            {
                var existingPlayer = players.Single(x => x.PlayerId == uniqueGuid);

                if (existingPlayer.Hitting.PA == 0)
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

        return players.Where(x => x.Hitting.PA != 0).ToList();
    }



    private (TeamGameInfo teamOfInterest, TeamGameInfo oppositionTeam) GetTeamsFromStats(GameChangerApiStats.Game stats, string teamId)
    {
        var teamOfInterest = stats.Teams.First(x => x.Id == teamId);
        var oppositionTeam = stats.Teams.First(x => x.Id != teamId);

        var teamOfInterestName = StringUtils.TrimName(teamOfInterest.LongName);
        var oppositionTeamName = StringUtils.TrimName(oppositionTeam.LongName);

        Guid teamOfInterestUid = clubId.NewGuid(teamOfInterestName.ToLowerInvariant()).Id;
        Guid oppositionTeamUid = clubId.NewGuid(oppositionTeamName.ToLowerInvariant()).Id;


        var isDraw = (teamOfInterest.Stats.Offense.R == oppositionTeam.Stats.Offense.R);
        var teamOfInterestInfo = new TeamGameInfo
        {
            UnqiueTeamId = teamOfInterestUid,
            TeamShortId = StringUtils.GetShortId(teamOfInterestUid, teamOfInterestName.ToLowerInvariant()),
            GameChangerTeamId = teamOfInterest.Id,
            TeamName = teamOfInterestName,
            IsHome = teamOfInterest.IsHome,
            Runs = teamOfInterest.Stats.Offense.R,
            Result = isDraw ? "D" : (teamOfInterest.Stats.Offense.R > oppositionTeam.Stats.Offense.R ? "W" : "L")
        };

        var oppositionTeamInfo = new TeamGameInfo
        {
            UnqiueTeamId = oppositionTeamUid,
            TeamShortId = StringUtils.GetShortId(oppositionTeamUid, oppositionTeamName.ToLowerInvariant()),
            GameChangerTeamId = oppositionTeam.Id,
            TeamName = oppositionTeamName,
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


