using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PuppeteerSharp;
using RealSoftware.Reviews.WebScraper.Abstractions;
using RealSoftware.Reviews.WebScraper.Models;
using RealSoftware.Reviews.WebScraper.Service;

namespace RealSoftware.Reviews.WebScraper.Scraper
{
    public class FacebookSearchScraper : ScraperBase<FacebookSearchOptions, List<SearchResultItemModel>>
    {
        public const string REVIEWS_URL = "https://www.facebook.com/public?query={0}&type=pages&init=dir&nomc=0";
        public const string POST_URL = "https://facebook.com/{0}";

        public FacebookSearchScraper(IScraperCache cache, IPage page) : base(cache, page)
        {
        }

        protected override async Task AfterLoadAsync(IPage page, FacebookSearchOptions options)
        {
            const string moreReviewsBtn = "#browse_end_of_results_footer";
            string content = "";
            bool hasMoreReviewsButton = false;

            do
            {
                await page.EvaluateFunctionAsync("window.__realsft.scroll");
                content = await page.GetContentAsync();
                hasMoreReviewsButton = await page.EvaluateFunctionAsync<bool>("window.__realsft.exists", moreReviewsBtn);
                Thread.Sleep(500);
            }
            while (!hasMoreReviewsButton);

        }

        protected override async Task LoadAsync(IPage page, FacebookSearchOptions options)
        {
            var pageurl = string.Format(REVIEWS_URL, WebUtility.UrlEncode(options.SearchText));
            var response = await page.GoToAsync(pageurl);

            if (!response.Ok)
            {
                throw new Exception($"Web site did not return a 200 status. Site response: {response.StatusText} ");
            }
        }

        public override Task<List<SearchResultItemModel>> ScrapeData(HtmlDocument document, FacebookSearchOptions options)
        {
            const string reviewsElementId = "BrowseResultsContainer";
            var reviewsChilderen = document.GetElementbyId(reviewsElementId).SelectNodes("./div");

            var scrollResultsChildren = document.DocumentNode.SelectNodes("//div[contains(@id,'fbBrowseScrollingPagerContainer')]/div/div");

            return Task.FromResult(reviewsChilderen.Concat(scrollResultsChildren).Select(GetSearchResultItemModelFromHtmlNode).Where(x => x != null).ToList());
        }

        internal SearchResultItemModel GetSearchResultItemModelFromHtmlNode(HtmlNode node)
        {
            if (node.InnerText.Contains("End of results")) return null;

            try
            {
                var attJsonData = node.Attributes.AttributesWithName("data-bt").First().DeEntitizeValue;
                // var b = JsonSerializer.Serialize(new AttribData { id = "123456" });
                var res = JsonSerializer.Deserialize<AttribData>(attJsonData);


                var postDateNodes = node.SelectSingleNode("./div/div/div/a");

                if (postDateNodes == null) return null;

                var pageLink = postDateNodes.Attributes.AttributesWithName("href").First().Value;
                var name = postDateNodes.Attributes.AttributesWithName("aria-label").First().Value;

                return new SearchResultItemModel
                {
                    Name = name,
                    Url = pageLink,
                    Id = res.id.ToString()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parse search node");
                return null;
            }
            return null;



            // if (postDateNodes == null || postDateNodes.Count() == 0) return null;

            // var postDateNode = postDateNodes.First();
            // var postUrl = postDateNode.Attributes.AttributesWithName("href").First().Value;
            // var postId = postUrl.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();

            // var profileNode = node.SelectNodesContainingClass("profileLink");
            // string cleanedName = CleanInnerText(profileNode.First().InnerText);

            // var postDateText = CleanInnerText(postDateNode.SelectNodesContainingClass("timestampContent").First().InnerText);
            // var postDate = DateTime.Parse(postDateText);

            // var contentNode = node.SelectSingleNode(".//div[@data-testid='post_message']");
            // string contentText = null;
            // if (contentNode != null)
            // {
            //     var reviewContentLines = contentNode == null ? null : contentNode.ChildNodes.Select(x => CleanInnerText(x.InnerText)).Where(x => x != string.Empty);
            //     contentText = string.Join("\r\n", reviewContentLines);
            // }

            // var review = new ReviewModel
            // {
            //     Id = postId,
            //     ReviewerName = cleanedName,
            //     ReviewUrl = string.Format(POST_URL, postId),
            //     Date = postDate,
            //     Content = contentText
            // };

            return null;
        }
    }

    public class FacebookSearchOptions
    {
        public string SearchText { get; set; }

    }

    public class AttribData
    {
        public long id { get; set; }
    }

    public class SearchResultItemModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}