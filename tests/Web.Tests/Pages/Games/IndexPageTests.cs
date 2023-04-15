using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using STKBC.Stats.Repositories;
using STKBC.Stats.Services;

namespace STKBC.Tests.Pages.Games;


public class IndexPageTests
{





    [Fact]
    public void IndexPage_Should_CorrectlyMapGameViewList()
    {
        var uniqueIdGenerator = new UniqueIdGenerator();

        var gameRepository = new InMemoryGameRepository(new List<Stats.Data.Models.Game>
        {
            new Stats.Data.Models.Game{
                Id = uniqueIdGenerator.NewDeterministicId("game1").Id,
                HomeTeamId = uniqueIdGenerator.NewDeterministicId("home-team-1").Id,
                HomeTeam = "Home Team 1",
                HomeTeamRuns = 10,
                AwayTeamId = uniqueIdGenerator.NewDeterministicId("away-team-2").Id,
                AwayTeam = "Away Team 2",
                AwayTeamRuns = 5,
                LeagueId = uniqueIdGenerator.NewDeterministicId("league-1").Id,
                SeasonId = uniqueIdGenerator.NewDeterministicId("season-1").Id,
                GradeId = uniqueIdGenerator.NewDeterministicId("grade-1").Id,
                GameDate = new DateTime(2023, 5, 4)
            }
        });

        var seasonRepository = new InMemorySeasonRepository(new List<Stats.Data.Models.Season>
        {
            new Stats.Data.Models.Season{
                Id = uniqueIdGenerator.NewDeterministicId("season-1").Id,
                Name = "2023"
            },
            new Stats.Data.Models.Season{
                Id = uniqueIdGenerator.NewDeterministicId("season-2").Id,
                Name = "2022"
            },
        });

        var leagueRepository = new InMemoryLeagueRepository(new List<Stats.Data.Models.League>
        {
            new Stats.Data.Models.League{
                Id = uniqueIdGenerator.NewDeterministicId("league-1").Id,
                Name = "Super League"
            },
            new Stats.Data.Models.League{
                Id = uniqueIdGenerator.NewDeterministicId("league-2").Id,
                Name = "League 2"
            },
        });

        var gradesRepository = new InMemoryGradeRepository(new List<Stats.Data.Models.Grade>
        {
            new Stats.Data.Models.Grade{
                Id = uniqueIdGenerator.NewDeterministicId("grade-1").Id,
                Name = "A Grade"
            },
            new Stats.Data.Models.Grade{
                Id = uniqueIdGenerator.NewDeterministicId("grade-2").Id,
                Name = "B Grade"
            },
            new Stats.Data.Models.Grade{
                Id = uniqueIdGenerator.NewDeterministicId("grade-3").Id,
                Name = "C Grade"
            }
        });

        var page = new STKBC.Stats.Pages.Games.IndexModel(
            gameRepository,
            seasonRepository,
            leagueRepository,
            gradesRepository,
            new Mock<ILogger<Stats.Pages.Games.IndexModel>>().Object
        );


        page.OnGet();

        Assert.Equal(page.Games.Count, 1);
        Assert.Equal(page.Games[0].GameId, uniqueIdGenerator.NewDeterministicId("game1").Id);
        Assert.Equal(page.Games[0].HomeTeamName, "Home Team 1");
        Assert.Equal(page.Games[0].HomeTeamId, uniqueIdGenerator.NewDeterministicId("home-team-1").Id);
        Assert.Equal(page.Games[0].AwayTeamName, "Away Team 2");
        Assert.Equal(page.Games[0].AwayTeamId, uniqueIdGenerator.NewDeterministicId("away-team-2").Id);
        Assert.Equal(page.Games[0].GameDate, "04-05-2023");
        Assert.Equal(page.Games[0].GradeName, "A Grade");
        Assert.Equal(page.Games[0].LeagueName, "Super League");
        Assert.Equal(page.Games[0].SeasonName, "2023");
        Assert.Equal(page.Games[0].Result, "Home Team 1 wins 10-5");




    }


    // [Fact]
    // public void IndexPage_Should_RenderAListOfGamesFromRepository()
    // {


    //     var page = new STKBC.Stats.Pages.Games.IndexModel(
    //         new InMemoryGameRepository(),
    //         new InMemorySeasonRepository(),
    //         new InMemoryLeagueRepository(),
    //         new InMemoryGradeRepository(),
    //         new Mock<ILogger<STKBC.Stats.Pages.Games.IndexModel>>().Object
    //     );

    //     var result = page.OnGet();
    //     Assert.NotNull(result);
    //     Assert.IsType<PageResult>(result);

    //     var pageResult = result as PageResult;
    //     pageResult.ToString




    //     var html = "";

    //     var parser = new HtmlParser();
    //     var document = parser.ParseDocument(html);

    //     // id="games-list-table"
    //     object tableRow = null;

    //     Assert.NotNull(tableRow);
    //     // <td>Game Date</td>
    //     Assert.Equal(tableRow[0].Text, "04-05-2023");
    //     // <td>Grade Name</td>
    //     Assert.Equal(tableRow[1].Text, "04-05-2023");
    //     // <td>League Name</td>
    //     Assert.Equal(tableRow[2].Text, "04-05-2023");
    //     // <td>Home Team Name</td>
    //     Assert.Equal(tableRow[3].Text, "04-05-2023");
    //     // <td>Away Team Name</td>
    //     Assert.Equal(tableRow[4].Text, "04-05-2023");
    //     // <td>Result</td
    //     Assert.Equal(tableRow[5].Text, "04-05-2023");



    // }
}