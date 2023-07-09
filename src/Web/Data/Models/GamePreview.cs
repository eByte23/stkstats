namespace STKBC.Stats.Data.Models
{
    public class GamePreview
    {
        public Guid? GameId { get; set; }
        public Guid? GradeId { get; set; }
        public TeamPreview? HomeTeam { get; set; }
        public TeamPreview? AwayTeam { get; set; }
        public Guid? LeagueId { get; set; }
    }

    public class TeamPreview
    {
        public Guid? TeamId { get; set; }
        public string? TeamName { get; set; }
        public List<PlayerPreview> Players { get; set; } = new();
    }
}