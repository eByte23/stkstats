using Newtonsoft.Json;
using StatSys.CoreStats;
using StatSys.CoreStats.Builders;

namespace CoreStats.Tests.Builders;


public class PlayerProfileBuilderTests
{
    private UniqueIdGenerator idGen = new UniqueIdGenerator();

    [Fact]
    public void TestAddingGamesPlayed()
    {

        var clubId = idGen.NewDeterministicId("STKBC");


        var builder = PlayerProfileBuilder.New(clubId, "John", "Smith", new List<string> { "1234" });


        var game1 = new StatSys.CoreStats.Models.GameData
        {
            GameId = clubId.NewGuid("game-1234").Id,
            GameChangerGameId = "game-1234",
            Date = new DateTime(2023, 6, 6, 0, 0, 0).ToString("yyyy-MM-dd"),
            AwayRuns = 1,
            HomeRuns = 8,
            GameShortId = "short-game-1234",
            GameUrl = "/games/short-game-1234",
            HomeAway = "Home",
            OppositionName = "Other Team",
            Location = "",
            Grade = "",
            OppositionId = "1234",
            Result = "W",
            Players = new List<StatSys.CoreStats.Models.PlayerData>{
                new StatSys.CoreStats.Models.PlayerData{
                    Name = "John  Smith ",
                    GameChangerIds = {"1234"},
                    UniqueId = builder.PlayerId,
                    ShortId ="33333",
                    Hitting = new StatSys.CoreStats.Models.HittingData{
                        PA =  4,
                        AB =  4,
                        H =  1,
                        TB =  1,
                        Singles =  1,
                        Doubles =  0,
                        Triples =  0,
                        HR =  0,
                        RBI =  1,
                        R =  1,
                        BB =  0,
                        SO =  1,
                        KL =  1,
                        SF =  0,
                        HBP =  0,
                        AVG =  ".250",
                        SLG =  ".250",
                        OPS =  ".500",
                        OBP =  ".250"
                    }
                }
            },
            SeasonId = "2023",
            SeasonName = "Winter 2023",
            TeamId = "our-team-12",
            TeamName = "Our Team",
            TeamShortId = "short-our-team-12",
            TeamUnqiueId = clubId.NewGuid("our-team-12").Id,
        };

        var game2 = new StatSys.CoreStats.Models.GameData
        {
            GameId = clubId.NewGuid("game-123456").Id,
            GameChangerGameId = "game-123456",
            Date = new DateTime(2023, 6, 13, 0, 0, 0).ToString("yyyy-MM-dd"),
            AwayRuns = 1,
            HomeRuns = 8,
            GameShortId = "short-game-12345",
            GameUrl = "/games/short-game-12345",
            HomeAway = "Home",
            OppositionName = "Other Team",
            Location = "",
            Grade = "",
            OppositionId = "1234",
            Result = "W",
            Players = new List<StatSys.CoreStats.Models.PlayerData>{
                new StatSys.CoreStats.Models.PlayerData{
                    Name = "John  Smith ",
                    GameChangerIds = {"1234"},
                    UniqueId = builder.PlayerId,
                    ShortId ="33333",
                    Hitting = new StatSys.CoreStats.Models.HittingData{
                        PA= 3,
                        AB= 1,
                        H= 0,
                        TB= 0,
                        Singles= 0,
                        Doubles= 0,
                        Triples= 0,
                        HR= 0,
                        RBI= 0,
                        R= 1,
                        BB= 2,
                        SO= 0,
                        KL= 0,
                        SF= 0,
                        HBP= 0,
                        AVG= ".000",
                        SLG= ".000",
                        OPS= ".667",
                        OBP= ".667"
                    }
                }
            },
            SeasonId = "2023",
            SeasonName = "Winter 2023",
            TeamId = "our-team-12",
            TeamName = "Our Team",
            TeamShortId = "short-our-team-12",
            TeamUnqiueId = clubId.NewGuid("our-team-12").Id,
        };


        var gamePlayed = PlayerProfileBuilder.GamePlayed.New(builder.PlayerId, game1);

        builder.AddGamePlayed(gamePlayed);

        var hittingTotal = builder._totalHitting;

        Assert.Equal(4, hittingTotal.PA);
        Assert.Equal(4, hittingTotal.AB);
        Assert.Equal(1, hittingTotal.H);
        Assert.Equal(1, hittingTotal.TB);
        Assert.Equal(1, hittingTotal.Singles);
        Assert.Equal(0, hittingTotal.Doubles);
        Assert.Equal(0, hittingTotal.Triples);
        Assert.Equal(0, hittingTotal.HR);
        Assert.Equal(1, hittingTotal.RBI);
        Assert.Equal(1, hittingTotal.R);
        Assert.Equal(0, hittingTotal.BB);
        Assert.Equal(1, hittingTotal.SO);
        Assert.Equal(1, hittingTotal.KL);
        Assert.Equal(0, hittingTotal.SF);
        Assert.Equal(0, hittingTotal.HBP);
        Assert.Equal(".250", hittingTotal.AVG);
        Assert.Equal(".250", hittingTotal.SLG);
        Assert.Equal(".500", hittingTotal.OPS);
        Assert.Equal(".250", hittingTotal.OBP);

        var gamePlayed2 = PlayerProfileBuilder.GamePlayed.New(builder.PlayerId, game2);

        builder.AddGamePlayed(gamePlayed2);

        var hittingTotal2 = builder._totalHitting;

        Assert.Equal(7, hittingTotal2.PA);
        Assert.Equal(5, hittingTotal2.AB);
        Assert.Equal(1, hittingTotal2.H);
        Assert.Equal(1, hittingTotal2.TB);
        Assert.Equal(1, hittingTotal2.Singles);
        Assert.Equal(0, hittingTotal2.Doubles);
        Assert.Equal(0, hittingTotal2.Triples);
        Assert.Equal(0, hittingTotal2.HR);
        Assert.Equal(1, hittingTotal2.RBI);
        Assert.Equal(2, hittingTotal2.R);
        Assert.Equal(2, hittingTotal2.BB);
        Assert.Equal(1, hittingTotal2.SO);
        Assert.Equal(1, hittingTotal2.KL);
        Assert.Equal(0, hittingTotal2.SF);
        Assert.Equal(0, hittingTotal2.HBP);
        Assert.Equal(".200", hittingTotal2.AVG);
        Assert.Equal(".200", hittingTotal2.SLG);
        Assert.Equal(".629", hittingTotal2.OPS);
        Assert.Equal(".429", hittingTotal2.OBP);

        var playerProfile = builder.Build();

        Assert.Equal(JsonConvert.SerializeObject(playerProfile.SeasonTotals.FirstOrDefault().Hitting), JsonConvert.SerializeObject(hittingTotal2));


    }




}