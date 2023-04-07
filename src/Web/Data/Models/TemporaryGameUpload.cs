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
            public int Ab { get; set; }
            public int Bb { get; set; }
            public int Cs { get; set; }
            public int Double { get; set; }
            public int Gdp { get; set; }
            public int Ground { get; set; }
            public int H { get; set; }
            public int Hbp { get; set; }
            public int Hr { get; set; }
            public int Kl { get; set; }
            public int Pickoff { get; set; }
            public int R { get; set; }
            public int Rbi { get; set; }
            public int Rchci { get; set; }
            public int Rcherr { get; set; }
            public int Sb { get; set; }
            public int Sf { get; set; }
            public int Sh { get; set; }
            public int So { get; set; }
            public int Triple { get; set; }
}

public class TemporaryTeam
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public List<TemporaryPlayer> Players { get; set; } = new List<TemporaryPlayer>();
    public bool Found { get; set; }
}