using STKBC.Stats.Repositories;

namespace STKBC.Tests.Repositories;

public class LeagueRepositoryTests
{
    [Fact]
    public void GetLeagues_Should_ReturnTwoLeagues()
    {
        var repo = new InMemoryLeagueRepository();

        var leagues = repo.GetLeagues();

        var mwbl = leagues.SingleOrDefault(x => x.Key == "mwbl");

        Assert.NotNull(mwbl);
        Assert.Equal(new Guid("1265c851-d526-44fc-bef4-55b2572c2b65"), mwbl.Id);
        Assert.Equal("Melbourne Winter Baseball League", mwbl.Name);
        Assert.Equal("MWBL", mwbl.ShortName);
        Assert.Equal("mwbl", mwbl.Key);

        var vicSummer = leagues.SingleOrDefault(x => x.Key == "vsbl");

        Assert.NotNull(vicSummer);
        Assert.Equal(new Guid("7b694799-7183-4754-a4f7-3c1f1921a1c7"), vicSummer.Id);
        Assert.Equal("Victorian Summer Baseball League", vicSummer.Name);
        Assert.Equal("VSBL", vicSummer.ShortName);
        Assert.Equal("vsbl", vicSummer.Key);


    }
}