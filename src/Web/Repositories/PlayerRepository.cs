using STKBC.Stats.Data.Models;

namespace STKBC.Stats.Repositories;

public interface IPlayerRepository
{
    Task CreatePlayerAsync(Player player);
    Task<Player?> GetPlayerAsync(Guid id);

    Task<List<Player>?> GetPlayersAsync();
   
}

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly List<Player> _players = new();

    public Task<Player?> GetPlayerAsync(Guid id)
    {
        return Task.FromResult(_players.FirstOrDefault(x => x.Id == id));
    }

    public Task<List<Player>?> GetPlayersAsync()
    {
        return Task.FromResult(_players);
    }

    public Task CreatePlayerAsync(Player player)
    {
        _players.Add(player);
        return Task.CompletedTask;
    }
}


public class LocalStorageFilePlayerRepository : IPlayerRepository
{
    private readonly RepoFileSystemStorage<Player> _repoFileSystemStorageHelper;

    public LocalStorageFilePlayerRepository(RepoFileSystemStorageHelper storageHelper)
    {
        _repoFileSystemStorageHelper = storageHelper.GetRepoFileSystemStorage<Player>();
    }

    public async Task<Player?> GetPlayerAsync(Guid id)
    {
        var players = await _repoFileSystemStorageHelper.GetAllAsync();


        return players.SingleOrDefault(x=> x.Id == id);
    }

    public async Task<List<Player>?> GetPlayersAsync()
    {
        return await _repoFileSystemStorageHelper.GetAllAsync();
    }

    public async Task CreatePlayerAsync(Player player)
    {
        var players = await _repoFileSystemStorageHelper.GetAllAsync();

        players.Add(player);

        await _repoFileSystemStorageHelper.SaveAllAsync(players);
    }
}