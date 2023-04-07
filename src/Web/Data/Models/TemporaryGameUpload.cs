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
    public TemporaryPlayerBattingStats? Batting { get; set; }
    public object? Fielding { get; set; }
    public object? Pitching { get; set; }
    public bool Found { get; set; }
}

public class TemporaryPlayerBattingStats
{
    public int Pa { get; set; }
    public int Ab { get; set; }
    public int Hits { get; set; }
    public int Singles { get; set; }
    public int Doubles { get; set; }
    public int Triples { get; set; }
    public int Hr { get; set; }
    public int RChErr { get; set; }
    public int Bb { get; set; }
    public int Hbp { get; set; }
    public int So { get; set; }
    public int SacFly { get; set; }
    public int Cs { get; set; }
    public int Runs { get; set; }
    public int Rbi { get; set; }
    public int Sb { get; set; }
}

public class TemporaryTeam
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public List<TemporaryPlayer> Players { get; set; } = new List<TemporaryPlayer>();
    public bool Found { get; set; }
}