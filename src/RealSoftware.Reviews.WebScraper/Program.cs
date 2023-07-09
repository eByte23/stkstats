using System.Reflection.Metadata.Ecma335;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using PuppeteerSharp;
using RealSoftware.Reviews.WebScraper.Models;
using RealSoftware.Reviews.WebScraper.Scraper;
using RealSoftware.Reviews.WebScraper.Service;
using Newtonsoft.Json.Linq;

namespace RealSoftware.Reviews.WebScraper
{

    public class GameChangerAccount
    {
        public List<GameChangerTeam> Teams { get; set; } = new List<GameChangerTeam>();

        public List<GameChangerGame> Games { get; set; } = new List<GameChangerGame>();
    }

    public class GameChangerTeam
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Link { get; set; }
    }

    public class GameChangerGame
    {
        public string? Id { get; set; }
        public string? TeamId { get; set; }
        public string? Name { get; set; }
        public string? Link { get; set; }
        public string? Date { get; set; }
        public string? StatsFile { get; set; }
    }






    class Program
    {
        public static async Task Main(string[] args)
        {
            var cache = new ScraperCache(@"/Users/elijahbate/Personal/Dev/stats/cache");

            var opts = new GameChangerOptions
            {
                Username = "stkildabaseball@gmail.com",
                Password = "Saints1879!"
            };

            string sampledataPath = Path.Combine("..", "..", "sampledata");
            var options = new LaunchOptions
            {
                Headless = true,
                DefaultViewport = new ViewPortOptions
                {
                    Height = 1080,
                    Width = 1920
                },
                ExecutablePath = "/Users/elijahbate/Personal/Dev/stats/.local-chromium/MacOS-1069273/chrome-mac/Chromium.app/Contents/MacOS/Chromium"

            };
            Console.WriteLine("Downloading chromium");

            var projectDirecotry = "/Users/elijahbate/Personal/Dev/stats";
            var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions
            {
                Path = @$"{projectDirecotry}/.local-chromium"
            });
            //             using var browserFetcher = new BrowserFetcher();
            // await browserFetcher.DownloadAsync();
            // await using var browser = await Puppeteer.LaunchAsync(
            //     new LaunchOptions { Headless = true });
            // await using var page = await browser.NewPageAsync();

            await browserFetcher.DownloadAsync();


            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                var teamScraper = new GameChangerTeamsScraper(cache, page);
                var teamsResult = await teamScraper.RunAsync(new GameChangerOptions
                {
                    Username = opts.Username,
                    Password = opts.Password,
                    SkipLogin = false
                }, $"game-changer-teams-{DateTime.Now.ToString("yyyy-MM-dd")}", "gamechanger-teams", true);

                File.WriteAllText("download/teams.json", JsonConvert.SerializeObject(teamsResult, Formatting.Indented));

                var scheduleScraper = new GameChangerGameScheduleScraper(cache, page);

                var gamesToFetch = new List<GameDownloadHistoryItem>();

                var notStartedGameIds = new List<string>();

                var gameIdsAndStreamIds = new Dictionary<string, string>();

                foreach (var team in teamsResult)
                {
                    var (scheduleRes, teamScheduleJson) = await scheduleScraper.RunAsync(new GameChangerGameScheduleScraperOptions
                    {
                        Username = opts.Username,
                        Password = opts.Password,
                        SkipLogin = true,
                        TeamId = team.Id,
                        TeamLink = team.Link,
                    }, $"game-changer-team-{team.Id}-{DateTime.Now.ToString("yyyy-MM-dd")}", "gamechanger-schedule");

                    gamesToFetch.AddRange(scheduleRes.GameIds.Select((gameId) => new GameDownloadHistoryItem
                    {
                        TeamId = team.Id,
                        GameId = gameId
                    }));

                    if (!Directory.Exists($"download/teams/{team.Id}"))
                    {

                        Directory.CreateDirectory($"download/teams/{team.Id}");
                    }

                    File.WriteAllText($"download/teams/{team.Id}/games.json", JsonConvert.SerializeObject(scheduleRes, Formatting.Indented));
                    File.WriteAllText($"download/teams/{team.Id}/team-schedule-data.json", JObject.Parse(teamScheduleJson).ToString(Formatting.Indented));

                    var teamSchedule = JsonConvert.DeserializeObject<GameChanger.Parser.GameChangerTeamSchedule>(File.ReadAllText($"download/teams/{team.Id}/team-schedule-data.json"));
                    var okGames = teamSchedule.TeamEvents.Where(x => x.PlayStatus == "CPT").Select(x => (x.GameId, x.StreamId)).ToList();

                    foreach (var (gameId, streamId) in okGames)
                    {
                        gameIdsAndStreamIds[gameId] = streamId;
                    }

                    notStartedGameIds.AddRange(teamSchedule.TeamEvents.Where(x => x.PlayStatus != "CPT").Select(x => x.GameId));
                }



                var gameDownloadHistory = GameDownloadHistory.LoadGameDownlaodHistory();
                var gamesNotAlreadyDownloaded = gamesToFetch
                    .Where(g => !gameDownloadHistory.History.Any(h => h.GameId == g.GameId && h.TeamId == g.TeamId))
                    .Where(g => !notStartedGameIds.Contains(g.GameId))
                    .ToList();
                var gamesScraper = new GameChangerGameStatsScraper(cache, page);


                foreach (var item in gamesNotAlreadyDownloaded.GroupBy(x => x.TeamId))
                {
                    var gameIds = gameIdsAndStreamIds.Where(x => item.Any(y => x.Key == y.GameId)).ToDictionary(x => x.Key, x => x.Value);

                    var gamesResults = await gamesScraper.RunAsync(new GameChangerGameStatsScraperOptions
                    {
                        Username = opts.Username,
                        Password = opts.Password,
                        SkipLogin = true,
                        GameIds = gameIds,
                    }, $"game-changer-games-{item.Key}{DateTime.Now.ToString("yyyy-MM-dd")}", "gamechanger-game-stats", true);


                    foreach (var game in gamesResults.GameStats)
                    {
                        if (!Directory.Exists($"download/teams/{item.Key}/games"))
                        {
                            Directory.CreateDirectory($"download/teams/{item.Key}/games");
                        }

                        File.WriteAllText($"download/teams/{item.Key}/games/{game.Id}.json", JsonConvert.SerializeObject(game, Formatting.Indented));
                    }

                    gameDownloadHistory.History.AddRange(item);
                    // File.WriteAllText($"teams/{item.Key}/game-data-{DateTime.Now.ToString("yyyy-MM-dd")}.json", JsonConvert.SerializeObject(scheduleRes, Formatting.Indented));
                }


                gameDownloadHistory.SaveGameDownloadHistory();










                // var scraper = new GameChangerStatsDownloader(cache, page);

                // var res = await scraper.RunAsync(gameChangerOptions, $"gamechanger-{DateTime.Now.ToString("yyyy-MM-dd")}", "gamechanger");
            }
        }
    }
}



public class GameDownloadHistoryItem
{
    public string TeamId { get; set; }
    public string GameId { get; set; }
}

public class GameDownloadHistory
{
    public List<GameDownloadHistoryItem> History { get; set; } = new List<GameDownloadHistoryItem>();


    public static GameDownloadHistory LoadGameDownlaodHistory()
    {
        if (File.Exists("download/game-download-history.json"))
        {
            return JsonConvert.DeserializeObject<GameDownloadHistory>(File.ReadAllText("download/game-download-history.json"));
        }
        else
        {
            return new GameDownloadHistory();
        }
    }

    public void SaveGameDownloadHistory()
    {
        File.WriteAllText("download/game-download-history.json", JsonConvert.SerializeObject(this, Formatting.Indented));
    }
}