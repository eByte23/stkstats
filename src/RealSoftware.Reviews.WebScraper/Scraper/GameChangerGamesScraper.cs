using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using PuppeteerSharp;
using RealSoftware.Reviews.WebScraper.Abstractions;
using RealSoftware.Reviews.WebScraper.Models;
using RealSoftware.Reviews.WebScraper.Service;

namespace RealSoftware.Reviews.WebScraper.Scraper
{
    public class GameChangerGameStatsScraperOptions : GameChangerOptions
    {
        public Dictionary<string, string> GameIds { get; set; } = new();
    }

    public class Result
    {
        public List<GameData> GameStats { get; set; } = new List<GameData>();
    }

    public class GameData
    {
        public string Id { get; set; }
        public string AbsoluteGameUrl { get; set; }
        public string StatsJson { get; set; }
        public string StreamJson { get; set; }
        public string StreamId { get; set; }
    }



    public class GameChangerGameStatsScraper : GameChangerBaseScraper<GameChangerGameStatsScraperOptions, Result>
    {
        public GameChangerGameStatsScraper(IScraperCache cache, IPage page) : base(cache, page)
        {
        }

        protected override async Task BeforeLoadAsync(IPage page, GameChangerGameStatsScraperOptions options)
        {
            if (options.SkipLogin)
                return;

            await base.BeforeLoadAsync(page, options);
        }

        public override async Task<Result> ScrapeData(HtmlDocument document, GameChangerGameStatsScraperOptions options)
        {
            var results = new Result();

            foreach (var gameId in options.GameIds)
            {
                var gameStatsDataUrl = $"https://gc.com/game-{gameId.Key}/stats.json";
                var statsResult = await Page.GoToAsync(gameStatsDataUrl);

                var statsHtmlDoc = await LoadHtmlDocument(Page);
                var statsJsonData = statsHtmlDoc.DocumentNode.InnerText;

                var streamUrl = $"https://push.gamechanger.io/push/game/{gameId.Key}/stream/{gameId.Value}?index=0&sabertooth_aware=true";
                var streamResult = await Page.GoToAsync(streamUrl);

                var streamHtmlDoc = await LoadHtmlDocument(Page);
                var streamJsonData = streamHtmlDoc.DocumentNode.InnerText;

                var absoluteGameUrl = $"https://gc.com/game-{gameId.Key}";
                results.GameStats.Add(new GameData
                {
                    Id = gameId.Key,
                    StreamId = gameId.Value,
                    AbsoluteGameUrl = absoluteGameUrl,
                    StatsJson = statsJsonData,
                    StreamJson = streamJsonData,
                });
            }

            return results;
        }

        public async Task<HtmlDocument> LoadHtmlDocument(IPage page)
        {
            var pageContent = await page.GetContentAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(pageContent);
            return htmlDoc;
        }
    }
}