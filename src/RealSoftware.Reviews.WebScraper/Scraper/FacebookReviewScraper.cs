using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PuppeteerSharp;
using RealSoftware.Reviews.WebScraper.Abstractions;
using RealSoftware.Reviews.WebScraper.Models;
using RealSoftware.Reviews.WebScraper.Service;

namespace RealSoftware.Reviews.WebScraper.Scraper
{
    public class FacebookReviewScraper : ReviewsScraperBase<FacebookReviewsLoadOptions>
    {
        public const string REVIEWS_URL = "https://www.facebook.com/pg/{0}/reviews/";
        public const string POST_URL = "https://facebook.com/{0}";

        public FacebookReviewScraper(IScraperCache cache, IPage page) : base(cache, page)
        {
        }

        protected override async Task AfterLoadAsync(IPage page, FacebookReviewsLoadOptions options)
        {
            const string moreReviewsBtn = ".uiMorePagerPrimary";
            string content = "";
            bool hasMoreReviewsButton = true;

            do
            {
                await page.EvaluateFunctionAsync("window.__realsft.scroll");
                content = await page.GetContentAsync();
                hasMoreReviewsButton = await page.EvaluateFunctionAsync<bool>("window.__realsft.exists", moreReviewsBtn);
                Thread.Sleep(500);
            }
            while (hasMoreReviewsButton);

        }

        protected override async Task LoadAsync(IPage page, FacebookReviewsLoadOptions options)
        {
            var pageurl = string.Format(REVIEWS_URL, options.PageId);
            var response = await page.GoToAsync(pageurl);

            if (!response.Ok)
            {
                throw new Exception($"Web site did not return a 200 status. Site response: {response.StatusText} ");
            }
        }

        public override Task<List<ReviewModel>> ScrapeData(HtmlDocument document, FacebookReviewsLoadOptions options)
        {
            const string reviewsElementId = "recommendations_tab_main_feed";
            var reviewsContainerElement = Document.GetElementbyId(reviewsElementId);

            return Task.FromResult(reviewsContainerElement.ChildNodes.Select(GetReviewModelFromHtmlNode).Where(x => x != null).ToList());
        }


        internal ReviewModel GetReviewModelFromHtmlNode(HtmlNode node)
        {
            var postDateNodes = node.SelectNodes(".//div[@data-testid='story-subtitle']/span/span/a");

            if (postDateNodes == null || postDateNodes.Count() == 0) return null;

            var postDateNode = postDateNodes.First();
            var postUrl = postDateNode.Attributes.AttributesWithName("href").First().Value;
            var postId = postUrl.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();

            var profileNode = node.SelectNodesContainingClass("profileLink");
            string cleanedName = CleanInnerText(profileNode.First().InnerText);

            var postDateText = CleanInnerText(postDateNode.SelectNodesContainingClass("timestampContent").First().InnerText);
            var postDate = DateTime.Parse(postDateText);

            var contentNode = node.SelectSingleNode(".//div[@data-testid='post_message']");
            string contentText = null;
            if (contentNode != null)
            {
                var reviewContentLines = contentNode == null ? null : contentNode.ChildNodes.Select(x => CleanInnerText(x.InnerText)).Where(x => x != string.Empty);
                contentText = string.Join("\r\n", reviewContentLines);
            }

            var review = new ReviewModel
            {
                Id = postId,
                ReviewerName = cleanedName,
                ReviewUrl = string.Format(POST_URL, postId),
                Date = postDate,
                Content = contentText
            };

            return review;
        }


    }

    public class FacebookReviewsLoadOptions
    {
        public string PageId { get; set; }

    }
}