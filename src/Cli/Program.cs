using GameChanger.Parser;
using Newtonsoft.Json;
using StatSys.CoreStats.Mappers;
using StatSys.CoreStats.Models;

namespace STKBC.Cli;


public class Program
{

    public static void Main(string[] args)
    {
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

        // var teamInfo = JsonConvert.DeserializeObject<List<TeamInfo>>(File.ReadAllText(teamsFilePath));

        List<GameData> gameOverviews = new List<GameData>();
        Dictionary<string, TeamOverview> teams = new Dictionary<string, TeamOverview>();


        foreach (var gameFilePath in filesToProcess)
        {
            var rawFileText = File.ReadAllText(gameFilePath);
            var gameInfo = JsonConvert.DeserializeObject<GameInfo>(rawFileText);

            DirectoryInfo? teamDir = Directory.GetParent(Path.GetDirectoryName(gameFilePath));
            var teamId = teamDir.Name;
            var teamSchedule = Path.Combine(teamDir.FullName, "team-schedule-data.json");

            var teamScheduleData = JsonConvert.DeserializeObject<GameChangerTeamSchedule>(File.ReadAllText(teamSchedule));

            var stats = JsonConvert.DeserializeObject<GameChanger.Parser.GameChangerApiStats.Game>(gameInfo.StatsJson);

            if (gameInfo.Id == "6496d2a47c0001710c00071f")
            {
                Console.WriteLine("Found it");
            }
            var outputGameData = new ToGameMapper().Map(teamId, gameInfo.Id, gameInfo.AbsoluteGameUrl, stats, teamScheduleData);
            if (outputGameData.OppositionName == "TBD" && outputGameData.Players.Count == 0)
            {
                Console.WriteLine("Game Id: '{0}', may be an incomplete game. Game Url: '{1}'", outputGameData.GameChangerGameId, outputGameData.GameUrl);
            }

            if (outputGameData == null || (outputGameData.AwayRuns == 0 && outputGameData.HomeRuns == 0 && outputGameData.Players.Count == 0)) continue;


            gameOverviews.Add(outputGameData);

            if (!teams.ContainsKey(teamId))
            {
                teams.Add(teamId, new TeamOverview
                {
                    TeamId = teamId,
                    TeamName = outputGameData.TeamName,
                    ShortId = outputGameData.TeamShortId,
                    SeasonId = outputGameData.SeasonId,
                    SeasonName = outputGameData.SeasonName,

                });
            }

            var team = teams[teamId];
            team.Games.Add((outputGameData.GameShortId, outputGameData.Date));

            File.WriteAllText($"game-output/{outputGameData.GameShortId}.json", JsonConvert.SerializeObject(outputGameData));
        }

        teams = teams.OrderBy(x => x.Value.TeamName).OrderByDescending(x => x.Value.SeasonName).ToDictionary(x => x.Key, x => x.Value);

        File.WriteAllText("teams.json", JsonConvert.SerializeObject(teams));


        var recentGames = gameOverviews.Where(x => DateTime.Parse(x.Date) > DateTime.Now.AddMonths(-2)).OrderByDescending(x => x.Date).Take(10).ToList();

        File.WriteAllText("recent-games.json", JsonConvert.SerializeObject(recentGames));


        var players = new GamesToIndividualPlayersStatsMapper().Map(gameOverviews);

        foreach (var player in players)
        {
            File.WriteAllText($"player-output/{player.ShortId}.json", JsonConvert.SerializeObject(player));
        }

        File.WriteAllText("players.json", JsonConvert.SerializeObject(players.Select(x => x.ShortId).ToList()));
        File.WriteAllText("games.json", JsonConvert.SerializeObject(gameOverviews.Select(x => x.GameShortId).ToList()));

        // serialize the game stats
        // map game stats to output style
        // write to file
        // write to line processed.txt









    }



}


public class TeamOverview
{
    public string ShortId { get; set; }
    public string TeamId { get; set; }
    public string TeamName { get; set; }
    public string SeasonName { get; set; }
    public string SeasonId { get; set; }

    public List<(string fileName, string date)> Games { get; set; } = new();
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