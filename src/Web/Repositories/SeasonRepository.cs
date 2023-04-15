using STKBC.Stats.Data.Models;

namespace STKBC.Stats.Repositories;

public interface ISeasonRepository
{
    List<Season> GetSeasons();
}

public class InMemorySeasonRepository : ISeasonRepository
{
    public static Season Summer2022_2023 => new()
    {
        Id = new Guid("2ee324f9-fd99-4ef6-9495-9ec53b99bcae"),
        Key = "summer-2022-2023",
        Name = "Summer 2022/23",
        StartDate = new DateTime(2022, 10, 1),
        LeagueId = InMemoryLeagueRepository.VSBL.Id,
    };

    public static Season Winter2022 => new()
    {

        Id = new Guid("923ee822-dfa9-43c7-9e0c-a3191f164593"),
        Key = "winter-2022",
        Name = "Winter 2022",
        StartDate = new DateTime(2022, 04, 23),
        LeagueId = InMemoryLeagueRepository.MWBL.Id,
    };

    public static Season Winter2023 => new()
    {
        Id = new Guid("7c6c9e96-3daa-4d0a-9d04-d6b25d34ce96"),
        Key = "winter-2023",
        Name = "Winter 2023",
        StartDate = new DateTime(2023, 04, 23),
        LeagueId = InMemoryLeagueRepository.MWBL.Id,
    };

    private readonly List<Season> _memory;

    public InMemorySeasonRepository(List<Season>? memory = null)
    {
        this._memory = memory ?? new()
        {
            InMemorySeasonRepository.Summer2022_2023,
            InMemorySeasonRepository.Winter2022,
            InMemorySeasonRepository.Winter2023,
        };
    }

    public List<Season> GetSeasons()
    {
        return _memory.OrderByDescending(x => x.StartDate).ToList();
    }
}