using STKBC.Stats.Repositories;
using STKBC.Stats.Data.Models;

namespace STKBC.Tests.Repositories;

public class SeasonRepositoryTests
{
    [Fact]
    public void GetSeasons_Should_ReturnThreeSeasons()
    {
        var repo = new InMemorySeasonRepository();

        var seasons = repo.GetSeasons();

        var winter2022 = seasons.SingleOrDefault(x => x.Key == "winter-2022");

        Assert.NotNull(winter2022);
        Assert.Equivalent(new Season
        {
            Id = new Guid("923ee822-dfa9-43c7-9e0c-a3191f164593"),
            Key = "winter-2022",
            Name = "Winter 2022",
            StartDate = new DateTime(2022, 04, 23),
            LeagueId = InMemoryLeagueRepository.MWBL.Id,
        }, winter2022);

        var summer2022_2023 = seasons.SingleOrDefault(x => x.Key == "summer-2022-2023");

        Assert.NotNull(summer2022_2023);
        Assert.Equivalent(new Season
        {
            Id = new Guid("2ee324f9-fd99-4ef6-9495-9ec53b99bcae"),
            Key = "summer-2022-2023",
            Name = "Summer 2022/23",
            StartDate = new DateTime(2022, 10, 1),
            LeagueId = InMemoryLeagueRepository.VSBL.Id,
        }, summer2022_2023);


        var winter2023 = seasons.SingleOrDefault(x => x.Key == "winter-2023");

        Assert.NotNull(winter2023);
        Assert.Equivalent(new Season
        {
            Id = new Guid("7c6c9e96-3daa-4d0a-9d04-d6b25d34ce96"),
            Key = "winter-2023",
            Name = "Winter 2023",
            StartDate = new DateTime(2023, 04, 23),
            LeagueId = InMemoryLeagueRepository.MWBL.Id,
        }, winter2023);
    }

    [Fact]
    public void GetSeasons_Should_ReturnSeasonsInOrderNewestToOldest()
    {
        var repo = new InMemorySeasonRepository();

        var seasons = repo.GetSeasons();


        Assert.True(seasons[0].Key == "winter-2023");
        Assert.True(seasons[1].Key == "summer-2022-2023");
        Assert.True(seasons[2].Key == "winter-2022");

    }
}