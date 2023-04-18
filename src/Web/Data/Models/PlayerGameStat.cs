public class PlayerGameStat
{
    // Fk GameId + PlayerId
    public Guid? PlayerId { get; set; }
    public Guid? GameId { get; set; }
    public DateTime? GameDate { get; set; }
    public string? PlayersTeamName { get; set; }
    public string? PlayersTeamId { get; set; }

    public string? AgainstTeamName { get; set; }
    public string? AgainstTeamId { get; set; }

    public PlayerGameHittingStat? Hitting { get; set; }



}

public class PlayerGameHittingStat
{

    public int? AB { get; set; }
    public int? Runs { get; set; }
    public int? H { get; set; }
    public int? Doubles { get; set; }
    public int? Triples { get; set; }
    public int? HR { get; set; }
    public int? RBI { get; set; }
    public int? BB { get; set; }
    public int? SO { get; set; }
    public int? SB { get; set; }
    public int? CS { get; set; }
    public int? AVG { get; set; }
    public int? OBP { get; set; }
    public int? SLG { get; set; }
    public int? OPS { get; set; }
}