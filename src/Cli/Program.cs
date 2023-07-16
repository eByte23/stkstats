using System.Security.Cryptography.X509Certificates;
using Cli;
using GameChanger.Parser;
using Newtonsoft.Json;
using StatSys.CoreStats;
using StatSys.CoreStats.Builders;
using StatSys.CoreStats.Mappers;
using StatSys.CoreStats.Models;

namespace STKBC.Cli;


public class Program
{

    public static void Main(string[] args)
    {
        var _idGenerator = new UniqueIdGenerator();

        var clubId = _idGenerator.NewDeterministicId("STKBC");
        // const string Path1 = "/workspace/stats/data/old-games/c res/St.Kilda 2019 MWBL C Resv Grade vs Doncaster 05_04_19 Stats.xml";
        // var fileText = File.ReadAllText(Path1);
        // var game = Stats.Parser.ParserUtil.Deserialize(fileText);

        const string dir = "/Users/elijahbate/Personal/Dev/stats/src/RealSoftware.Reviews.WebScraper/download/teams/";
        const string teamsFilePath = "/Users/elijahbate/Personal/Dev/stats/src/RealSoftware.Reviews.WebScraper/download/teams.json";

        var processedFilePaths = File.ReadAllLines("processed.txt").ToList();

        var folders = Directory.EnumerateDirectories(dir);

        var filesToProcess = new List<string>();
        foreach (var folderPath in folders)
        {
            var gamesFolder = Path.Combine(folderPath, "games");

            if (!Directory.Exists(gamesFolder)) continue;

            var files = Directory.EnumerateFiles(gamesFolder).ToList();
            filesToProcess.AddRange(files.Where(x => !processedFilePaths.Contains(x)));
        }

        var teamInfo = JsonConvert.DeserializeObject<List<TeamInfo>>(File.ReadAllText(teamsFilePath));

        var teamPageViewBuilders = teamInfo.Select(x =>
        {
            var teamSchedule = Path.Combine(dir, x.Id, "team-schedule-data.json");
            var teamScheduleData = JsonConvert.DeserializeObject<GameChangerTeamSchedule>(File.ReadAllText(teamSchedule));


            var teamPage = TeamBuilder.New(clubId, new TeamMetadata
            {
                SeasonId = teamScheduleData.Team.SeasonId,
                SeasonName = teamScheduleData.Team.SeasonName,
                ShortId = "",
                TeamId = teamScheduleData.Team.Id,
                TeamName = x.Name,
            });

            return teamPage;

        }).ToList();


        List<GameData> gameOverviews = new List<GameData>();


        foreach (var gameFilePath in filesToProcess)
        {
            var rawFileText = File.ReadAllText(gameFilePath);
            var gameInfo = JsonConvert.DeserializeObject<GameInfo>(rawFileText);

            DirectoryInfo? teamDir = Directory.GetParent(Path.GetDirectoryName(gameFilePath));
            var teamId = teamDir.Name;
            var teamSchedule = Path.Combine(teamDir.FullName, "team-schedule-data.json");

            var teamScheduleData = JsonConvert.DeserializeObject<GameChangerTeamSchedule>(File.ReadAllText(teamSchedule));

            var stats = JsonConvert.DeserializeObject<GameChanger.Parser.GameChangerApiStats.Game>(gameInfo.StatsJson);


            var game = new ToGameMapper().Map(teamId, gameInfo.Id, gameInfo.AbsoluteGameUrl, stats, teamScheduleData);
            if (game.OppositionName == "TBD" && game.Players.Count == 0)
            {
                Console.WriteLine("Game Id: '{0}', may be an incomplete game. Game Url: '{1}'", game.GameChangerGameId, game.GameUrl);
            }

            if (game == null || (game.AwayRuns == 0 && game.HomeRuns == 0 && game.Players.Count == 0)) continue;


            gameOverviews.Add(game);

            var teamPageBuilder = teamPageViewBuilders.FirstOrDefault(x => x.TeamReferenceId == teamId);

            teamPageBuilder.AddGamePlayed(new TeamBuilder.GamePlayed
            {
                GameDate = game.Date,
                GameId = game.GameId,
                GameLocation = game.Location,
                GameOpponent = game.OppositionName,
                GameShortId = game.GameShortId,
                // Hitting = ,
                Players = game.Players.Select(x => new TeamBuilder.PlayerGamePlayed
                {
                    Name = x.Name,
                    ShortId = x.ShortId,
                    Hitting = x.Hitting,
                    PlayerId = x.UniqueId
                }).ToList(),
                OppositionRuns = game.HomeAway == "Home" ? game.AwayRuns : game.HomeRuns,
                Runs = game.HomeAway == "Home" ? game.HomeRuns : game.AwayRuns,
                Result = game.Result
            });

            WriteFileUtils.FolderSafeWriteAllText($"game-output/{game.GameShortId}.json", JsonConvert.SerializeObject(game));
        }

        var teams = teamPageViewBuilders
            .Select(x => x.Build())
            .OrderBy(x => x.TeamName)
            .OrderByDescending(x => x.SeasonName)
            .ToList();

        foreach (var item in teams)
        {
            WriteFileUtils.FolderSafeWriteAllText($"team-output/{item.TeamShortId}.json", JsonConvert.SerializeObject(item));
        }

        var recentGames = gameOverviews.Where(x => DateTime.Parse(x.Date) > DateTime.Now.AddMonths(-2)).OrderByDescending(x => x.Date).Take(10).ToList();
        WriteFileUtils.FolderSafeWriteAllText("recent-games.json", JsonConvert.SerializeObject(recentGames));

        var players = new GamesToIndividualPlayersStatsMapper().Map(gameOverviews);

        foreach (var player in players)
        {
            WriteFileUtils.FolderSafeWriteAllText($"player-output/{player.ShortId}.json", JsonConvert.SerializeObject(player));
        }

        WriteFileUtils.FolderSafeWriteAllText("teams.json", JsonConvert.SerializeObject(teams.Select(x =>
        {
            var wins = x.Games.Count(x => x.Result == "W");
            var losses = x.Games.Count(x => x.Result == "L");
            var draws = x.Games.Count(x => x.Result == "D");


            return new TeamMetadata
            {
                TeamId = x.ReferenceTeamId,
                Record = draws == 0 ? $"{wins}-{losses}" : $"{wins}-{losses}-{draws}",
                SeasonId = x.SeasonId,
                SeasonName = x.SeasonName,
                ShortId = x.TeamShortId,
                TeamName = x.TeamName,
                Games = x.Games.Select(x => (x.ShortId, x.Date)).OrderByDescending(x => x.Date).ToList()
            };

        })));
        WriteFileUtils.FolderSafeWriteAllText("players.json", JsonConvert.SerializeObject(players.Select(x => x.ShortId).ToList()));
        WriteFileUtils.FolderSafeWriteAllText("games.json", JsonConvert.SerializeObject(gameOverviews.Select(x => x.GameShortId).ToList()));


    }



}

public class GameInfo
{
    public string? Id { get; set; }
    public string? AbsoluteGameUrl { get; set; }
    public string? StatsJson { get; set; }
    public string? StreamJson { get; set; }
}

public class TeamInfo
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Link { get; set; }
}