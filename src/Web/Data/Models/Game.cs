namespace STKBC.Stats.Data.Models;

public class Game
{
    public Guid? Id { get; set; }
    public string? HomeTeam { get; set; }
    public string? AwayTeam { get; set; }

    public Guid? LeagueId { get; set; }
    public Guid GradeId { get; set; }


}