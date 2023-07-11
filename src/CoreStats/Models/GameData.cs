namespace StatSys.CoreStats.Models;

public class GameData
{
    public Guid GameId { get; set; }
    public Guid TeamUnqiueId { get; set; }
    public string? TeamShortId { get; set; }
    public string? TeamId { get; set; }
    public string? TeamName { get; set; }
    public string? Grade { get; set; }
    public string? HomeAway { get; set; }
    public string? OppositionName { get; set; }
    public string? OppositionId { get; set; }
    public string? SeasonName { get; set; }
    public string? SeasonId { get; set; }

    public string? Location { get; set; }
    public string Date { get; set; }
    // W, L, D
    public string? Result { get; set; }
    public int HomeRuns { get; set; }
    public int AwayRuns { get; set; }
    public List<PlayerData> Players { get; set; } = new();
    public string? GameChangerGameId { get; set; }
    public string? GameUrl { get; set; }
    public string? GameShortId { get; set; }
}
