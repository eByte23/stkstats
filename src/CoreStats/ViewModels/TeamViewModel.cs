using System.Xml;
namespace StatSys.CoreStats.ViewModels;

public class TeamViewModel
{
    public Guid TeamId { get; set; }
    public required string ReferenceTeamId { get; set; }
    public required string TeamName { get; set; }
    public required string TeamShortId { get; set; }
    public string? Grade { get; set; }
    public required string SeasonId { get; set; }
    public required string SeasonName { get; set; }
    public List<GameTotals> Games { get; set; } = new();
    public List<TeamPlayerTotals> Players { get; set; } = new();
    public Models.HittingData TeamTotalHitting { get; set; } = new();


    public class GameTotals
    {
        public Guid GameId { get; set; }
        public required string ShortId { get; set; }
        public required string Name { get; set; }
        public string? Date { get; set; }
        public Models.HittingData Hitting { get; set; } = new();
        public string? OppositionTeam { get; set; }
        public int HomeRuns { get; set; }
        public int AwayRuns { get; set; }
        public string? Result { get; set; }
    }

    public class TeamPlayerTotals
    {
        public Guid PlayerId { get; set; }
        public required string ShortId { get; set; }
        public required string Name { get; set; }
        public int GamesPlayed { get; set; }
        public Models.HittingData Hitting { get; set; } = new();
    }
}