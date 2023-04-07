namespace STKBC.Stats.Data.Models;


public class TemporaryGameUpload
{
    public Guid? Id { get; set; }
    public Guid? ImportRequestId { get; set; }


    public Guid? LeagueId { get; set; }
    public Guid? GradeId { get; set; }
    public Guid? FileId { get; set; }
    public string? FileName { get; set; }


    public DateTime? GameDate { get; set; }
    public TemporaryTeam? HomeTeam { get; set; }
    public TemporaryTeam? AwayTeam { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TemporaryPlayer
{
    public string? DisplayName { get; set; }
    public Guid? PlayerId { get; set; }
    public object? Batting { get; set; }
    public object? Fielding { get; set; }
    public object? Pitching { get; set; }
    public bool Found { get; set; }
}

public class TemporaryTeam
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public List<TemporaryPlayer> Players { get; set; } = new List<TemporaryPlayer>();
    public bool Found { get; set; }
}