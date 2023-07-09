using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RealSoftware.Reviews.WebScraper.Models;

namespace RealSoftware.Reviews.WebScraper.Abstractions
{
    public interface IScrapeReviews<TLoadOpt> : IScraper<TLoadOpt, List<ReviewModel>> where TLoadOpt : class
    {
        // List<ReviewModel> GetReviews(TLoadOpt options);
    }

    public interface IScraper<TLoadOpt, TData> where TLoadOpt : class where TData : class
    {
        Task<TData> ScrapeData(HtmlDocument document, TLoadOpt options);
        Task<TData> RunAsync(TLoadOpt options, string cacheKey, string type, bool ignoreCache, bool skipCached);
    }
}