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