using STKBC.Stats.Data.Models;

namespace STKBC.Stats.Repositories;


public interface IGameRepository
{
    List<Stats.Data.Models.Game> GetGames();
    bool AddGame(Data.Models.Game game);
}


public class InMemoryGameRepository : IGameRepository
{
    private List<Game> _games;

    public InMemoryGameRepository(List<Data.Models.Game>? games = null)
    {
        _games = games ?? new List<Data.Models.Game>();
    }

    public List<Data.Models.Game> GetGames()
    {
        return _games;
    }

    public bool AddGame(Data.Models.Game game)
    {
        _games.Add(game);
        return true;
    }
}

public class GameRepository : IGameRepository
{
    // private readonly StatsContext _context;

    public GameRepository()
    {
        // _context = context;
    }

    public bool AddGame(Game game)
    {
        throw new NotImplementedException();
    }

    public List<Stats.Data.Models.Game> GetGames()
    {
        throw new NotImplementedException();
    }
}