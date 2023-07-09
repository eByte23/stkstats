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

    public class GameChangerGameScheduleScraperOptions : GameChangerOptions
    {
        public string TeamId { get; set; }
        public string TeamLink { get; set; }
    }

    public class ListOfGameIds
    {
        public string TeamId { get; set; }
        public List<string> GameIds { get; set; }
    }


    public class GameChangerGameScheduleScraper : GameChangerBaseScraper<GameChangerGameScheduleScraperOptions, Tuple<ListOfGameIds, string>>
    {
        public GameChangerGameScheduleScraper(IScraperCache cache, IPage page) : base(cache, page)
        {
        }

        protected override async Task BeforeLoadAsync(IPage page, GameChangerGameScheduleScraperOptions options)
        {
            if (options.SkipLogin)
                return;

            await base.BeforeLoadAsync(page, options);
        }

        protected override async Task LoadAsync(IPage page, GameChangerGameScheduleScraperOptions options)
        {

            var url = $"https://gc.com{options.TeamLink}/schedule/games";

            await page.GoToAsync(url);
        }

        public async Task<HtmlDocument> LoadHtmlDocument(IPage page)
        {
            var pageContent = await page.GetContentAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(pageContent);
            return htmlDoc;
        }


        public async Task<List<string>> GetGameSchedule(HtmlDocument htmlDoc)
        {
            var gameRows = htmlDoc.DocumentNode.SelectNodes("//table[contains(@class,\"scheduleEventsTable\")]/tbody/tr[contains(@class,\"game\")]");
            var gameIds = new List<string>();

            if (gameRows == null)
            {
                return gameIds;
            }

            foreach (var item in gameRows)
            {
                if (item.GetAttributeValue("data-event-type", "") != "game")
                {
                    continue;
                }

                var gameId = item.GetAttributeValue("data-id", "");
                gameIds.Add(gameId);
            }

            return gameIds;
        }


        public string GetTeamScheduleFromInlineJson(HtmlDocument document)
        {
            var x = document.DocumentNode.SelectSingleNode("//script[contains(text(),\"page.initialize($.parseJSON(\")]");
            var idx = x.InnerHtml.IndexOf("page.initialize($.parseJSON(\"");
            var a = x.InnerText.Substring(idx + 29);
            var idx2 = a.IndexOf("\"), true");
            var b = a.Substring(0, idx2).Replace("\\u0022", "\"");

            return b;
        }

        public override async Task<Tuple<ListOfGameIds, string>> ScrapeData(HtmlDocument document, GameChangerGameScheduleScraperOptions options)
        {


            var teamGameScheduleJson = GetTeamScheduleFromInlineJson(document);


            return Tuple.Create(new ListOfGameIds
            {
                TeamId = options.TeamId,
                GameIds = await GetGameSchedule(document)
            }, teamGameScheduleJson);



        }



    }


}