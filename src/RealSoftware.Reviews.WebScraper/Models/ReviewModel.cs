using System;

namespace RealSoftware.Reviews.WebScraper.Models
{
    public class ReviewModel
    {
        public string Id { get; set; }
        public string ReviewUrl { get; set; }
        public string ReviewerName { get; set; }
        public string Content { get; set; }

        public int? Stars { get; set; }
        public DateTime Date { get; set; }
    }
}