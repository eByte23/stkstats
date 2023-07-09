using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using RealSoftware.Reviews.WebScraper.Abstractions;
using RealSoftware.Reviews.WebScraper.Models;
using RealSoftware.Reviews.WebScraper.Service;

namespace RealSoftware.Reviews.WebScraper.Scraper
{
    public class GoogleReviewsScraper : ReviewsScraperBase<GoogleReviewsLoadOptions>
    {
        public const string REVIEWS_URL = "https://maps.google.com";
        public const string POST_URL = "https://www.google.com/maps/contrib/{0}/place/{1}";
        string[] sessionPb = null;
        private bool sessionFound = false;

        public GoogleReviewsScraper(IScraperCache cache, IPage page) : base(cache, page)
        {
        }

        protected override async Task AfterLoadAsync(IPage page, GoogleReviewsLoadOptions options)
        {
            const string inputbox = "#searchboxinput";
            const string searchButton = "#searchbox-searchbutton";
            const string showMoreReviews = "button[jsaction='pane.reviewChart.moreReviews']";
            const string allReviewsPane = "div[aria-label='All reviews']";
            const string scrollPageSelector = ".section-scrollbox";
            const string loadingSelector = "div.section-scrollbox div.section-loading";

            // Console.WriteLine("HIT");
            await page.SetRequestInterceptionAsync(true);



            page.Request += async (s, req) =>
            {


                if (req.Request.Url.Contains("/preview/review/listentitiesreviews") && sessionPb == null)
                {
                    // Console.WriteLine(JsonConvert.SerializeObject(req.Request));
                    var pb = req.Request.Url.Split("pb=")[1];
                    Console.WriteLine("pb:"+pb);
                    sessionPb = pb.Split('!');
                    sessionFound = true;

                }




                await req.Request.ContinueAsync();
            };


            // Type into search and click search
            await page.TypeAsync(inputbox, options.Searchtext);
            await page.WaitForSelectorAsync(searchButton);
            await page.ClickAsync(searchButton);

            // Click All Reviews button
            await page.WaitForSelectorAsync(showMoreReviews);
            await page.ClickAsync(showMoreReviews);

            // Wait for reviews pane to display
            await page.WaitForSelectorAsync(allReviewsPane);

            string content = "";
            bool hasMoreReviewsButton = true;

            do
            {
                if (hasMoreReviewsButton)
                {

                    await page.EvaluateFunctionAsync("window.__realsft.scrollElement", scrollPageSelector);
                }
                content = await page.GetContentAsync();
                hasMoreReviewsButton = await page.EvaluateFunctionAsync<bool>("window.__realsft.exists", loadingSelector);
                // Thread.Sleep(5000);
            }
            while (!sessionFound);

        }

        static HttpClient _client = new HttpClient();

        public async Task<List<ReviewModel>> GetReviewsApiCall(int page, string[] sessionPb, GoogleReviewsLoadOptions options)
        {
            var p = (page - 1).ToString().PadRight(2, '0');


            var requestPb = sessionPb.Take(5).ToList();
            requestPb.Add($"1i{p}");
            requestPb.Add($"2i50");
            requestPb.AddRange(sessionPb.Skip(7));
            var pb = string.Join('!', requestPb);

            var url = string.Format("https://www.google.com/maps/preview/review/listentitiesreviews?authuser=0&hl=en&gl=au&pb={0}", pb);

            var req = new HttpRequestMessage
            {

                RequestUri = new Uri(url)
            };

            req.Headers.Add("authority", "www.google.com");
            req.Headers.Add("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"90\", \"Google Chrome\";v=\"90\"");
            req.Headers.Add("sec-ch-ua-mobile", "?0");
            req.Headers.Add("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36");
            req.Headers.Add("sec-fetch-site", "same-origin");
            req.Headers.Add("sec-fetch-mode", "cors");
            req.Headers.Add("sec-fetch-dest", "empty");
            req.Headers.Add("referer", "https://www.google.com/");
            //   -H 'accept-language: en,ja;q=0.9,de-DE;q=0.8,de;q=0.7,ca;q=0.6' \


            var res = await _client.SendAsync(req);

            string v = await res.Content.ReadAsStringAsync();
            v = v.Remove(0, 4);

            List<ReviewModel> reviews = new List<ReviewModel>();
            try
            {
                var t = JToken.Parse(v);

                foreach (var ct in t[2])
                {
                    var review = ParseResponseItemReview(ct, options);
                    reviews.Add(review);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(v);
            }



            return reviews;






        }


        public ReviewModel ParseResponseItemReview(JToken itemToken, GoogleReviewsLoadOptions options)
        {
            var url = itemToken[0][0];
            var name = itemToken[0][1];
            var review = itemToken[3];
            var stars = itemToken[4];
            var userId = itemToken[6];

            var postId = itemToken[10];
            var timestamp = itemToken[57];

            return new ReviewModel
            {
                Id = postId.ToString(),
                ReviewUrl = string.Format(POST_URL, userId, options.PlaceId),
                Content = review.ToString(),
                ReviewerName = name.ToString(),
                Stars = (int)stars,
                Date = DateTimeOffset.FromUnixTimeMilliseconds(((long)timestamp)).UtcDateTime
            };
        }







        protected override async Task LoadAsync(IPage page, GoogleReviewsLoadOptions options)
        {
            var response = await page.GoToAsync(REVIEWS_URL);

            if (!response.Ok)
            {
                throw new Exception($"Web site did not return a 200 status. Site response: {response.StatusText} ");
            }
        }

        public override async Task<List<ReviewModel>> ScrapeData(HtmlDocument document, GoogleReviewsLoadOptions options)
        {
            // const string reviewNodesSelector = "//div[@class='section-layout']/div[@data-review-id]";
            // var reviewsContainerElement = Document.DocumentNode.SelectNodes(reviewNodesSelector);

            // return Task.FromResult(reviewsContainerElement.Select(x => GetReviewModelFromHtmlNode(x, options)).Where(x => x != null).ToList());
            var reviews = new List<ReviewModel>();
            for (int i = 1; i < 50; i++)
            {
                var c = await GetReviewsApiCall(i, sessionPb, options);

                if (c.Count == 0) break;

                reviews.AddRange(c);
            }

            return reviews.GroupBy(x => x.Id).Select(x => x.First()).ToList();
        }


        internal ReviewModel GetReviewModelFromHtmlNode(HtmlNode node, GoogleReviewsLoadOptions options)
        {
            const string reviewContent = "section-review-text";
            const string starRatingSelector = ".//span[contains(@class,'section-review-stars')]";

            var starRatingNode = node.ChildNodes.Where(x => x.Attributes.AttributesWithName("aria-label").Any(x => !string.IsNullOrEmpty(x.Value) && x.Value?.ToUpperInvariant().Contains("STARS") == true)).SingleOrDefault();

            if (starRatingNode == null)
            {
                return default(ReviewModel);
            }


            // var starRatingNode = node.SelectSingleNode(starRatingSelector);
            var ratingString = starRatingNode.Attributes.AttributesWithName("aria-label").First().Value.Trim().First().ToString();
            var starRating = int.Parse(ratingString);

            if (starRating < options.MinStarRating) return null;

            var postId = node.Attributes.AttributesWithName("data-review-id").First().Value;

            var profileNode = node.SelectSingleNode(".//div[@class='section-review-title']/..");
            var postUrl = profileNode.Attributes.AttributesWithName("href").First().Value;

            string cleanedName = CleanInnerText(profileNode.SelectSingleNode(".//div[@class='section-review-title']/span").InnerText);

            var urlToGetUserIdFrom = postUrl.Contains('?') ? postUrl.Split('?')[0] : postUrl;
            var userId = urlToGetUserIdFrom.Split('/').Last();

            var contentNode = node.SelectNodesContainingClass(reviewContent, "span").First();

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
                ReviewUrl = string.Format(POST_URL, userId, options.PlaceId),
                // Date = postDate,
                Content = contentText
            };

            return review;
        }

    }

    public class GoogleReviewsLoadOptions
    {
        public string PlaceId { get; set; }
        public string Searchtext { get; set; }
        public int? MinStarRating { get; set; }
    }
}