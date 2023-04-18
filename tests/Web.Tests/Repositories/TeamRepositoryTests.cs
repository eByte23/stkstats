using STKBC.Stats.Data.Models;

namespace STKBC.Tests.Repositories;


public class TeamRepositoryTests
{
    private static List<Club> _memory = new List<Club>();

    public List<Club> GetTeams()
    {
        return _memory;
    }
}






// Automatically injest game.
// Set to pending Map team name home away to internal teams and continue.
// Game Changer Team Name => Season, Grade. 