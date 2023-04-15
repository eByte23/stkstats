using STKBC.Stats.Data.Models;

namespace STKBC.Stats.Repositories;


public interface ILeagueRepository
{
    List<Stats.Data.Models.League> GetLeagues();
}

public class InMemoryLeagueRepository : ILeagueRepository
{


    public static Stats.Data.Models.League MWBL => new()
    {
        Id = new Guid("1265c851-d526-44fc-bef4-55b2572c2b65"),
        Name = "Melbourne Winter Baseball League",
        ShortName = "MWBL",
        Key = "mwbl"
    };

    public static Stats.Data.Models.League VSBL => new()
    {
        Id = new Guid("7b694799-7183-4754-a4f7-3c1f1921a1c7"),
        Name = "Victorian Summer Baseball League",
        ShortName = "VSBL",
        Key = "vsbl"
    };


    private readonly List<League> _memory;

    public InMemoryLeagueRepository(List<Stats.Data.Models.League>? memory = null)
    {
        this._memory = memory ?? new List<Data.Models.League>{
            InMemoryLeagueRepository.MWBL,
            InMemoryLeagueRepository.VSBL,
        };
    }


    public List<Stats.Data.Models.League> GetLeagues()
    {


        return _memory;
    }
}