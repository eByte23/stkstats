using GameChanger.Parser;
using Newtonsoft.Json;

namespace Web.Tests;


public class GenerateGameDataTests
{



    [Fact]
    public void Test()
    {

        var gameStats = ParserUtil.ParseStatsFile("test_files/full-stats-game.json");


        var team = gameStats.Teams.Where(x => x.Players.Any(x => x.PlayerId == "648dc462f84f0136bb00000f")).First();
        var player = team.Players.First(x => x.Id == "648dc462f84f0136bb00000f");

        Assert.NotNull(player);

        Assert.Equal(1, player.Stats.Offense.BB);
        Assert.Equal(2, player.Stats.Offense.QAB);
        Assert.Equal("66.67%", player.Stats.Offense.QABPercentage);
        Assert.Equal(0, player.Stats.Offense._3B);


    }

}

public class GameData
{
    public string TeamId { get; set; }
    public string TeamName { get; set; }
    public string Grade { get; set; }
    public string HomeAway { get; set; }
    public string Opposition { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public string Result { get; set; }
    public int HomeRuns { get; set; }
    public int AwayRuns { get; set; }
    public List<PlayerData> Players { get; set; } = new();
}

public class PlayerData
{
    public string? UniqueId { get; set; }
    public string? ShortId { get; set; }
    public string? Name { get; set; }
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

    [JsonProperty("1B")]
    public string? _1B { get; set; }

    [JsonProperty("2B")]
    public string? _2B { get; set; }

    [JsonProperty("3B")]
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


// {
//     "team": "Team 1",
//     "grade": "D Res",
//     "homeAway": "H",
//     "opposition": "Team 2",
//     "location": "",
//     "date": "2017-04-01",
//     "result": "W",
//     "homeRuns": 2,
//     "awayRuns": 1,
//     "players": [
//         {
//             "id": "elijah-bate-12345sf",
//             "name": "Elijah Bate",
//             "hitting": {
//                 "pa": 3,
//                 "atBats": 3,
//                 "runs": 1,
//                 "hits": 2,
//                 "rbi": 1,
//                 "walks": 0,
//                 "strikeouts": 0,
//                 "avg": 0.667,
//                 "obp": 0.667,
//                 "slg": 0.667,
//                 "ops": 1.333
//             }
//         }
//     ]
// }