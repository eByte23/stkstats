namespace STKBC.Stats.Pages.Models.Partials
{
    public class PlayerPreview
    {
        public string? DisplayName { get; set; }
        public bool? Matched { get; set; }
        public HittingStatPreview? Hitting { get; set; }
        public Guid? TempId { get; set; }
        public Guid? MatchedId { get;  set; }
    }

    public class HittingStatPreview
    {

        public int? AB { get; set; }
        public int? Runs { get; set; }
        public int? H { get; set; }
        public int? Doubles { get; set; }
        public int? Triples { get; set; }
        public int? HR { get; set; }
        public int? RBI { get; set; }
        public int? BB { get; set; }
        public int? SO { get; set; }
        public int? SB { get; set; }
        public int? CS { get; set; }
        public int? AVG { get; set; }
        public int? OBP { get; set; }
        public int? SLG { get; set; }
        public int? OPS { get; set; }
    }
}