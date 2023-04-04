using STKBC.Stats.Data.Models;

namespace STKBC.Tests.Repositories;


public class TeamRepositoryTests
{
    private static List<Team> _memory = new List<Team>();

    public List<Team> GetTeams()
    {
        return _memory;
    }
}






// Automatically injest game.
// Set to pending Map team name home away to internal teams and continue.
// Game Changer Team Name => Season, Grade. 