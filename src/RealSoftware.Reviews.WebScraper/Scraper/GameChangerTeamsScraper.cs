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
    public class TeamInfo
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Link { get; set; }
    }

    public class GameChangerTeamsScraper : GameChangerBaseScraper<GameChangerOptions, List<TeamInfo>>
    {

        public GameChangerTeamsScraper(IScraperCache cache, IPage page) : base(cache, page)
        {

        }

        protected override async Task BeforeLoadAsync(IPage page, GameChangerOptions options)
        {
            if (options.SkipLogin)
                return;

            await base.BeforeLoadAsync(page, options);
        }

        public async Task<List<TeamInfo>> GetTeams(HtmlDocument htmlDoc)
        {
            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"menu\"]/li[contains(@class,\"teamsMenu\")]/ul/li/a");

            var teams = new List<TeamInfo>();

            foreach (var node in nodes)
            {
                if (node.SelectSingleNode("span[contains(@class,\"teamName\")]") == null)
                    continue;

                var team = new TeamInfo();
                team.Link = node.GetAttributeValue("href", "");
                team.Name = node.GetAttributeValue("title", "");

                var linkParts = team.Link.Split('-');
                team.Id = linkParts.Last();

                teams.Add(team);
            }

            return teams;
        }

        public override async Task<List<TeamInfo>> ScrapeData(HtmlDocument document, GameChangerOptions options)
        {
            var teams = await GetTeams(document);

            return teams;
        }
    }

}