using Newtonsoft.Json;

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

public class PlayerData
{
    // used for web slug format: <fname>-<last>-<last12-digist-uniqueid>
    // e.g. elijah-bate-29cd020d6b4b
    public string? ShortId { get; set; }


    // GUID unqiue by club and fname-lname
    public Guid UniqueId { get; set; }

    // fname lname
    public string? Name { get; set; }
    public List<string> GameChangerIds { get; set; } = new();
    public HittingData Hitting { get; set; } = new();

    // public PitchingData Pitching { get; set; } = new();
}


public class HittingData
{
    [JsonProperty("PA")]
    public string? PA { get; set; }

    [JsonProperty("AB")]
    public string? AB { get; set; }

    [JsonProperty("H")]
    public string? H { get; set; }

    [JsonProperty("TB")]
    public string? TB { get; set; }

    [JsonProperty("Singles")]
    public string? _1B { get; set; }

    [JsonProperty("Doubles")]
    public string? _2B { get; set; }

    [JsonProperty("Triples")]
    public string? _3B { get; set; }

    [JsonProperty("HR")]
    public string? HR { get; set; }

    [JsonProperty("RBI")]
    public string? RBI { get; set; }

    [JsonProperty("R")]
    public string? R { get; set; }

    [JsonProperty("BB")]
    public string? BB { get; set; }

    [JsonProperty("SO")]
    public string? SO { get; set; }

    [JsonProperty("SOL")]
    public string? KL { get; set; }

    [JsonProperty("AVG")]
    public string? AVG { get; set; }

    [JsonProperty("SLG")]
    public string? SLG { get; set; }

    [JsonProperty("OPS")]
    public string? OPS { get; set; }

    [JsonProperty("OBP")]
    public string? OBP { get; set; }
}