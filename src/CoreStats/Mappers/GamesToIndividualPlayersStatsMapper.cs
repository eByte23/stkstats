using System.Linq;
using GameChanger.Parser;
using Newtonsoft.Json;
using StatSys.CoreStats.Builders;
using StatSys.CoreStats.Models;

namespace StatSys.CoreStats.Mappers;

public class GamesToIndividualPlayersStatsMapper
{
    private UniqueIdGenerator _idGenerator = new UniqueIdGenerator();
    private readonly DeterministicGuid clubId;
    //
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
                PlayerId = p.UniqueId,
            }))
            .GroupBy(x => x.PlayerId)
            .ToList();
        // .Select(p =>
        // {
        //     var player = p.First().Player;
        //     var games = p.Select(x => x.Game).ToList();
        //     return new PlayerIndividualStats
        //     {

        //         UniqueId = player.UniqueId,
        //         FirstName = player.FirstName,
        //         LastName = player.LastName,
        //         GamesPlayed = games,
        //     };
        // })
        // .ToList();

        var individualPlayers = new List<PlayerProfile>();

        foreach (var player in gamesPlayedByPlayers)
        {
            var gpPlayer = player.First().Player;

            var builder = PlayerProfileBuilder.New(clubId, gpPlayer.FirstName, gpPlayer.LastName, gpPlayer.GameChangerIds);

            // if (gpPlayer == null)
            // {
            //     throw new Exception("Player not found in game");
            // }

            // var playerStas = new IndividualPlayerStats
            // {
            //     GamesPlayed = new(),
            //     UniqueId = player.Key,
            //     Name = gpPlayer.Name,
            //     ShortId = gpPlayer.ShortId,
            //     GameChangerIds = gpPlayer.GameChangerIds,
            //     SeasonTotals = new(),
            //     TotalHitting = new(),
            // };

            // var gamesPlayed = player.Select(x => x.Game).ToList();

            // foreach (var gamePlayed in gamesPlayed)
            // {

            //     if (gamePlayed.GameChangerGameId == "647b347db3ef03c70e0002c9" && playerStas.ShortId == "zoe-pow-7480f5249776")
            //     {
            //         Console.WriteLine("Found it");
            //     }

            //     var hitting = gpPlayer.Hitting;

            //     var battingAvg = HittingData.GetBattingAvg(hitting);

            //     if (CompareAverage(hitting.AVG, battingAvg))
            //     {
            //         throw new Exception("Batting average does not match");
            //     }

            //     var battingObp = HittingData.GetBattingObpPercentage(hitting);

            //     if (CompareAverage(hitting.OBP, battingObp))
            //     {
            //         throw new Exception("Batting OBP does not match");
            //     }

            //     var slg = HittingData.GetSluggingPercentage(hitting);

            //     if (CompareAverage(hitting.SLG, slg))
            //     {
            //         throw new Exception("Batting SLG does not match");
            //     }

            //     var ops = HittingData.GetOpsPercentage(battingObp, slg);

            //     if (CompareAverage(hitting.OPS, ops))
            //     {
            //         throw new Exception("Batting OPS does not match");
            //     }

            //     playerStas.TotalHitting.AddGame(hitting);

            //     var seasonId = gamePlayed.SeasonId;

            //     var seasonTotals = playerStas.SeasonTotals.FirstOrDefault(x => x.SeasonId == seasonId);

            //     if (seasonTotals == null)
            //     {
            //         seasonTotals = new SeasonTotal
            //         {
            //             SeasonId = seasonId,
            //             SeasonName = gamePlayed.SeasonName,
            //             GamesPlayed = 1,
            //             Hitting = new()
            //         };
            //         seasonTotals.Hitting.AddGame(hitting);
            //         playerStas.SeasonTotals.Add(seasonTotals);
            //     }
            //     else
            //     {
            //         seasonTotals.GamesPlayed++;
            //         seasonTotals.Hitting.AddGame(hitting);
            //     }




            // }

            // individualPlayers.Add(playerStas);


            // var playerGames = games.Where(g => g.Players.Any(p => p.UniqueId == player.UniqueId)).ToList();
            // player.GamesPlayed = playerGames.Count;
            // player.SeasonTotals = GetSeasonTotals(playerGames, currentSeasonId);
            // player.TotalHitting = GetTotalHitting(playerGames);

            individualPlayers.Add(builder.Build());
        }

        return individualPlayers;

    }

    private static bool CompareAverage(string? expectedAvg, double battingAvg)
    {
        string formattedAvg = HittingData.GetPercentageString(battingAvg);
        return formattedAvg != expectedAvg;
    }

    private object? GetTotalHitting()
    {
        return null;
    }
    private object? GetSeasonTotals()
    {
        return null;
    }


    internal class GamePlayed
    {
        public required GameData Game { get; set; }
        public required Guid PlayerId { get; set; }
        public required PlayerData Player { get; set; }
    }


}