namespace STKBC.Stats.Data.Models;

public class Game
{
    public Guid? Id { get; set; }
    public Guid? HomeTeamId { get; set; }
    public string? HomeTeam { get; set; }
    public int? HomeTeamRuns { get; set; }
    public Guid? AwayTeamId { get; set; }
    public string? AwayTeam { get; set; }
    public int? AwayTeamRuns { get; set; }

    public Guid? LeagueId { get; set; }
    public Guid? SeasonId { get; set; }
    public Guid? GradeId { get; set; }
    public DateTime? GameDate { get; set; }
}