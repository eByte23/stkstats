namespace STKBC.Stats.Pages.Models.Partials
{
    public class GamePreviewView
    {
        public Guid? GameId { get; set; }
        public Guid? GradeId { get; set; }
        public string? GradeName { get; set; }
        public TeamPreviewView? HomeTeam { get; set; }
        public string? HomeTeamName { get; set; }
        public TeamPreviewView? AwayTeam { get; set; }
        public string? AwayTeamName { get; set; }
    }

    public class TeamPreviewView
    {
        public Guid? GameId { get; set; }
        public Guid? TeamId { get; set; }
        public string? TeamName { get; set; }
        public List<PlayerPreviewView> Players { get; set; } = new();
    }
}