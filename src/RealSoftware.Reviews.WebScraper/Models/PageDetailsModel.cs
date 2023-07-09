namespace RealSoftware.Reviews.WebScraper.Models
{
    public class PageDetailsModel
    {
        public string PageUrl { get; set; }
        public string Name { get; set; }
        public string PageId { get; set; }
        public int Likes { get; set; }
        public string Suburb { get; set; }
        public string Address { get; set; }

        public string[] Info { get; set; }
    }
}