namespace StatSys.CoreStats.Models;

public class IndividualPlayerStats
{
    public Guid UniqueId { get; set; }
    public string? ShortId { get; set; }
    public string? Name { get; set; }
    public List<string> GameChangerIds { get; set; } = new();
    public List<GameData> GamesPlayed { get; set; } = new();

    public List<SeasonTotal> SeasonTotals { get; set; } = new();

    public HittingData TotalHitting { get; set; } = new();
}

public class SeasonTotal
{
    public string? SeasonId { get; set; }
    public string? SeasonName { get; set; }
    public int GamesPlayed { get; set; }
    public HittingData Hitting { get; set; } = new();
}
