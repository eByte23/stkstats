using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using StatSys.CoreStats.Models;

namespace StatSys.CoreStats.Builders
{
    public class TeamBuilder
    {

        private DeterministicGuid _clubId;
        private DeterministicGuid _teamId;
        private string _teamName;
        private string _teamReferenceId;
        private string _seasonName;
        private string _seasonId;
        private string _shortId;

        private TeamBuilder(DeterministicGuid clubId, DeterministicGuid teamId1, string teamName, string teamId2, string seasonName, string seasonId, string shortId)
        {
            this._clubId = clubId;
            this._teamId = teamId1;
            this._teamName = teamName;
            this._teamReferenceId = teamId2;
            this._seasonName = seasonName;
            this._seasonId = seasonId;
            this._shortId = shortId;
        }

        public List<GamePlayed> _gamesPlayed { get; set; } = new();
        // public Models.HittingData _totalHitting { get; set; } = new();
        // public Models.PitchingData TotalPitchimg { get; set; } = new();
        // public Models.FieldingData TotalFielding { get; set; } = new();

        public Guid TeamId => _teamId.Id;
        public string TeamReferenceId => _teamReferenceId;





        public static TeamBuilder New(DeterministicGuid clubId, Models.TeamMetadata teamMetadata)
        {

            var teamName = StringUtils.TrimName(teamMetadata.TeamName);
            var seasonName = StringUtils.TrimName(teamMetadata.SeasonName);
            var teamId = clubId.NewGuid(teamName.ToLowerInvariant());

            var shortId = StringUtils.GetShortId(teamId.Id, teamName.ToLowerInvariant());


            return new TeamBuilder(clubId, teamId, teamName, teamMetadata.TeamId, seasonName, teamMetadata.SeasonId, shortId);

        }

        public TeamBuilder AddGamePlayed(TeamBuilder.GamePlayed gamePlayed)
        {
            _gamesPlayed.Add(gamePlayed);



            return this;
        }



        public ViewModels.TeamViewModel Build()
        {


            var teamPlayerTotals = _gamesPlayed
                .SelectMany(x => x.Players)
                .GroupBy(x => x.PlayerId)
                .Select(x =>
                {
                    PlayerGamePlayed playerGamePlayed = x.First();
                    return new ViewModels.TeamViewModel.TeamPlayerTotals
                    {
                        Name = playerGamePlayed.Name,
                        ShortId = playerGamePlayed.ShortId,
                        PlayerId = playerGamePlayed.PlayerId,
                        GamesPlayed = x.Count(),
                        Hitting = x.Select(x => x.Hitting).Aggregate((x, y) => x.AddGame2(y))
                    };
                })
                .ToList();

            var totalHitting = new HittingData();

            if (teamPlayerTotals.Count > 0)
            {
                totalHitting = teamPlayerTotals.Select(x => x.Hitting).Aggregate((x, y) => x.AddGame2(y));
            }

            return new ViewModels.TeamViewModel
            {
                TeamId = _teamId.Id,
                TeamName = _teamName,
                ReferenceTeamId = _teamReferenceId,
                SeasonName = _seasonName,
                SeasonId = _seasonId,
                TeamShortId = _shortId,
                Games = _gamesPlayed.Select(x =>
                {
                    var hittingTotal = x.Players.Select(x => x.Hitting).Aggregate((x, y) => x.AddGame2(y));

                    return new ViewModels.TeamViewModel.GameTotals
                    {
                        ShortId = x.GameShortId,
                        Name = $"{_teamName} vs. {x.GameOpponent}",
                        OppositionTeam = x.GameOpponent,
                        Hitting = hittingTotal,
                        Date = x.GameDate,
                        GameId = x.GameId,
                        HomeRuns = x.Runs,
                        AwayRuns = x.OppositionRuns,
                        Result = x.Result,
                    };
                }).OrderByDescending(x => x.Date).ToList(),
                Players = teamPlayerTotals,
                TeamTotalHitting = totalHitting,

                // TotalHitting = _totalHitting,
            };


        }



        public class GamePlayed
        {
            public Guid GameId { get; set; }
            public string? GameDate { get; set; }
            public string? GameShortId { get; set; }
            public string? GameLocation { get; set; }
            public string? GameOpponent { get; set; }
            public int OppositionRuns { get; set; }
            public string? Result { get; set; }
            public int Runs { get; set; }

            public Models.HittingData Hitting { get; set; } = new();
            public List<PlayerGamePlayed> Players { get; set; } = new();

            public static GamePlayed New(Models.GameData game)
            {

                var totalHitting = game.Players.Select(x => x.Hitting).Aggregate((x, y) => x.AddGame2(y));


                return new GamePlayed
                {
                    GameId = game.GameId,
                    GameDate = game.Date,
                    GameLocation = game.Location,
                    GameOpponent = game.OppositionName,
                    GameShortId = game.GameShortId,
                    Hitting = totalHitting,
                    OppositionRuns = game.HomeAway == "Home" ? game.AwayRuns : game.HomeRuns,
                    Result = game.Result,
                    Runs = game.HomeAway == "Home" ? game.HomeRuns : game.AwayRuns,
                    Players = game.Players.Select(x => new PlayerGamePlayed
                    {
                        Name = x.Name,
                        PlayerId = x.UniqueId,
                        ShortId = x.ShortId,
                        Hitting = x.Hitting
                    }).ToList()
                };
            }
        }

        public class PlayerGamePlayed
        {
            public required string Name { get; set; }
            public required string ShortId { get; set; }
            public Guid PlayerId { get; set; }
            public HittingData? Hitting { get; set; }
        }
    }
}