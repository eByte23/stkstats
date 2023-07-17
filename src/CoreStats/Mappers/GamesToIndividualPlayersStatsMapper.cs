using StatSys.CoreStats.Builders;
using StatSys.CoreStats.Models;

namespace StatSys.CoreStats.Mappers;

public class GamesToIndividualPlayersStatsMapper
{
    private UniqueIdGenerator _idGenerator = new UniqueIdGenerator();
    private readonly DeterministicGuid clubId;
    
    public GamesToIndividualPlayersStatsMapper()
    {
        clubId = _idGenerator.NewDeterministicId("STKBC");
    }

    public List<PlayerProfile> Map(List<GameData> games)
    {
        var gamesPlayedByPlayers = games
            .SelectMany(x => x.Players.Select(p => new GamePlayed
            {
                Game = x,
                Player = p,
                PlayerId = p.PlayerId,
            }))
            .GroupBy(x => x.PlayerId)
            .ToList();

        var individualPlayers = new List<PlayerProfile>();

        foreach (var player in gamesPlayedByPlayers)
        {
            var gpPlayer = player.FirstOrDefault().Player;

            if (gpPlayer == null)
            {
                throw new Exception("Player not found in game");
            }

            var builder = PlayerProfileBuilder.New(clubId, gpPlayer.FirstName, gpPlayer.LastName, gpPlayer.GameChangerIds);

            var gamesPlayed = player.Select(x => x.Game).ToList();

            foreach (var gamePlayed in gamesPlayed)
            {
                var gp = PlayerProfileBuilder.GamePlayed.New(builder.PlayerId, gamePlayed);
                builder.AddGamePlayed(gp);
            }


            individualPlayers.Add(builder.Build());
        }

        return individualPlayers;

    }


    internal class GamePlayed
    {
        public required GameData Game { get; set; }
        public required Guid PlayerId { get; set; }
        public required PlayerData Player { get; set; }
    }


}