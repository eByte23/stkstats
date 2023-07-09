using System;
using System.Collections.Generic;
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
    public class FacebookPageDetailsScraper : ScraperBase<FacebookPageDetailsOptions, PageDetailsModel>
    {
        // public const string REVIEWS_URL = "https://www.facebook.com/public?query={0}&type=pages&init=dir&nomc=0";
        // public const string POST_URL = "https://facebook.com/{0}";

        public FacebookPageDetailsScraper(IScraperCache cache,IPage page) : base(cache,page)
        {
        }

        protected override async Task AfterLoadAsync(IPage page, FacebookPageDetailsOptions options)
        {
            const string Args = "#pagelet_growth_expanding_cta";
            if (await Page.EvaluateFunctionAsync<bool>("window.__realsft.exists", Args))
            {
                await Page.EvaluateFunctionAsync("window.__realsft.remove", Args);
            }
        }

        protected override async Task LoadAsync(IPage page, FacebookPageDetailsOptions options)
        {
            // var pageurl = string.Format(REVIEWS_URL, WebUtility.UrlEncode(options.SearchText));
            var response = await page.GoToAsync(options.PageUrl);

            if (!response.Ok)
            {
                throw new Exception($"Web site did not return a 200 status. Site response: {response.StatusText} ");
            }
        }

        public override Task<PageDetailsModel> ScrapeData(HtmlDocument document, FacebookPageDetailsOptions options)
        {
            var pageAppLinkNode = document.DocumentNode.SelectNodes("//meta").Where(x => x.Attributes.Any(x => x.Name == "property" && x.Value == "al:ios:url")).FirstOrDefault();
            var pageId = pageAppLinkNode.Attributes.AttributesWithName("content").First().Value.Split("?id=").Last();
            var details = new PageDetailsModel
            {
                PageUrl = options.PageUrl,
                PageId = pageId
            };

            // <meta property="al:ios:url" content="fb://page/?id=407680272610056">
            // var right coloumn id ="PagesProfileHomeSecondaryColumnPagelet";
            var id = "PagesLikesCountDOMID";
            var profileDetailsId = $"PagesProfileAboutInfoPagelet_{pageId}";

            var nameNode = document.GetElementbyId("seo_h1_tag");
            details.Name = nameNode.ChildNodes.First().InnerText;

            var pagelikesNode = document.GetElementbyId(id);

            var rightColNode = document.GetElementbyId("PagesProfileHomeSecondaryColumnPagelet");

            var info = document.DocumentNode.SelectSingleNode("//*[@id='PagesProfileHomeSecondaryColumnPagelet']/div/div[2]/div/div[3]");

            if (info != null)
            {
                var collectedArr = info.Descendants().Select(x => CleanInnerText(x.InnerText)).Where(x =>
                {
                    if (string.IsNullOrWhiteSpace(x)) return false;

                    // AboutSee all\r\nAbout\r\nAbout\r\n\r\nSee all\r\nSee all\r\nSee all
                    if (
                        x.Contains("about", StringComparison.InvariantCultureIgnoreCase)
                    || x.Contains("See all", StringComparison.InvariantCultureIgnoreCase)
                    || x.Contains("Page transparency", StringComparison.InvariantCultureIgnoreCase)
                    || x.Contains("Get Directions", StringComparison.InvariantCultureIgnoreCase)
                    || x.Contains("Closed now", StringComparison.InvariantCultureIgnoreCase)
                    || x.Contains("Opens at", StringComparison.InvariantCultureIgnoreCase)
                    || x.Contains("See More", StringComparison.InvariantCultureIgnoreCase)

                    //            "Page transparencySee More",
                    // "·",
                    // Get Directions
                    // "Closed now"
                    // "Opens at 08:30Closed now",
                    )
                        return false;



                    return true;
                }).GroupBy(x => x).Select(x => x.Key);
                details.Info = collectedArr.ToArray();
            }

            if (pagelikesNode == null)
            {
                var n = rightColNode.Descendants().Where(x => x.InnerText.Contains("people like this")).FirstOrDefault();
            }
            else
            {
                string s = new string(pagelikesNode.InnerText.Where(c => Char.IsDigit(c)).ToArray());
                details.Likes = int.Parse(s);
            }

            // people like this


            return Task.FromResult(details);
            // return  reviewsContainerElement.ChildNodes.Select(GetSearchResultItemModelFromHtmlNode).Where(x => x != null).ToList();
        }
    }

    public class FacebookPageDetailsOptions
    {

        public string PageUrl { get; set; }
    }

}