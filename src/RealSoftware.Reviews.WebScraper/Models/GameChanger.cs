using RealSoftware.Reviews.WebScraper.Abstractions;

namespace RealSoftware.Reviews.WebScraper.Models
{
    public class GameChangerOptions : IGameChangerOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool SkipLogin { get; set; }



    }
}