using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PuppeteerSharp;
using RealSoftware.Reviews.WebScraper.Models;
using RealSoftware.Reviews.WebScraper.Service;

namespace RealSoftware.Reviews.WebScraper.Abstractions
{

    public abstract class ReviewsScraperBase<TLoadOpt> : ScraperBase<TLoadOpt, List<ReviewModel>>, IScrapeReviews<TLoadOpt> where TLoadOpt : class
    {
        protected ReviewsScraperBase(IScraperCache cache, IPage page) : base(cache, page)
        {
        }
    }

    public abstract class ScraperBase<TLoadOpt, TData> : IScraper<TLoadOpt, TData> where TLoadOpt : class where TData : class
    {
        protected string PageContent { get; private set; }
        public IScraperCache Cache { get; }
        protected IPage Page { get; private set; }
        protected HtmlDocument Document { get; private set; }

        protected ScraperBase(IScraperCache cache, IPage page)
        {
            Cache = cache;
            Page = page;
        }

        protected virtual Task BeforeLoadAsync(IPage page, TLoadOpt options)
            => Task.CompletedTask;

        protected virtual Task AfterLoadAsync(IPage page, TLoadOpt options)
            => Task.CompletedTask;

        protected virtual Task LoadAsync(IPage page, TLoadOpt options)
            => Task.CompletedTask;

        public abstract Task<TData> ScrapeData(HtmlDocument document, TLoadOpt options);

        public async Task<TData> RunAsync(TLoadOpt options, string key, string type, bool ignoreCache = false, bool skipCached = false)
        {
            var cacheitem = await Cache.Get<TData>(key);
            var htmlDoc = new HtmlDocument();

            Console.WriteLine("HIT");
            if (ignoreCache || cacheitem.Item1 == null)
            {
                await BeforeLoadAsync(Page, options);
                await LoadAsync(Page, options);

                var jsHelper = File.ReadAllText("./js/webutils.js");

                await Page.AddScriptTagAsync(new AddTagOptions
                {
                    Content = jsHelper,
                    Type = "text/javascript"
                });
                try
                {
                    Console.WriteLine("HIT");

                    await AfterLoadAsync(Page, options);
                    PageContent = await Page.GetContentAsync();
                    htmlDoc.LoadHtml(PageContent);
                }
                catch (Exception e)
                {
                    var path = $@"/Users/elijahbate/Personal/Dev/stats/errors/{DateTime.Now.ToString("yyyy-MM-dd-hh:mm")}.jpg";
                    await Page.ScreenshotAsync(path);
                    throw e;
                }
            }
            else if (skipCached)
            {
                Console.WriteLine("Skiping process of cached, skip cached flag used");
                return cacheitem.Item2;
            }
            else
            {
                var htmlData = await Cache.LoadHtmlFile(cacheitem.Item1);
                htmlDoc.LoadHtml(htmlData);
            }



            Document = htmlDoc;
            var data = await ScrapeData(Document, options);

            var id = await Cache.Save(key, type, data, htmlDoc.DocumentNode.OuterHtml);

            Console.WriteLine("Stored key '{0}' as id '{1}' in cache", key, id);

            return data;
        }

        public string GetPageContent() => PageContent;

        public static string CleanInnerText(string inner)
        {
            var stripedValue = inner.Replace("\r\n", " ");
            string[] value = stripedValue.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (value == null || value.Length == 0) return string.Empty;

            return string.Join(" ", value);
        }
    }
}