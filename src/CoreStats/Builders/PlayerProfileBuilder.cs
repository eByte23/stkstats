namespace StatSys.CoreStats.Builders;




//   private UniqueIdGenerator _idGenerator = new UniqueIdGenerator();
//     private readonly DeterministicGuid clubId;
//     //
//     public ToGameMapper()
//     {
//         clubId = _idGenerator.NewDeterministicId("STKBC");
//     }


//     public static string GetShortId(Guid id, params string[] parts)
//     {
//         var last12 = new string(id.ToString().TakeLast(12).ToArray());

//         return string.Join("-", parts.Select(p => p.Replace("  ", " ").Replace(" ", "-")).Append(last12)).Replace("`", "").Replace("'", "").ToLower();
//     }

public class PlayerProfileBuilder
{

    private DeterministicGuid? _clubId;
    private DeterministicGuid _playerId;
    private string? _firstName;
    private string? _lastName;
    private string _shortId;
    private List<string> _referencePlayerIds = new();
    public List<GamePlayed> _gamesPlayed { get; set; } = new();
    public Models.HittingData _totalHitting { get; set; } = new();
    // public Models.PitchingData TotalPitchimg { get; set; } = new();
    // public Models.FieldingData TotalFielding { get; set; } = new();


    private PlayerProfileBuilder(DeterministicGuid clubId, DeterministicGuid playerId, string firstName, string lastName, List<string> referencePlayerIds, string shortId)
    {
        _clubId = clubId;
        _playerId = playerId;
        _firstName = firstName;
        _lastName = lastName;
        _shortId = shortId;
        _referencePlayerIds = referencePlayerIds;
    }

    public static PlayerProfileBuilder New(DeterministicGuid clubId, string firstName, string lastName, List<string> referencePlayerIds)
    {
        var cleanFirstName = StringUtils.TrimName(firstName);
        var cleanLastName = StringUtils.TrimName(lastName);

        string fullName = StringUtils.BuildName(cleanFirstName, cleanLastName);
        var playerId = clubId.NewGuid(fullName.ToLower());

        var shortId = StringUtils.GetShortId(playerId.Id, cleanFirstName, cleanLastName);



        return new PlayerProfileBuilder(clubId, playerId, firstName, lastName, referencePlayerIds, shortId);
    }

    public Guid PlayerId => _playerId.Id;

    public PlayerProfileBuilder AddGamePlayed(GamePlayed gamePlayed)
    {
        if (_gamesPlayed.Any(x => x.GameId == gamePlayed.GameId))
        {
            throw new Exception($"Game {gamePlayed.GameId} already added");
        }

        _gamesPlayed.Add(gamePlayed);
        _totalHitting = _totalHitting.AddGame2(gamePlayed.Hitting);


        return this;
    }

    public PlayerProfile Build()
    {
        var seasonTotals = GetSeasonStats();



        return new PlayerProfile
        {
            FullName = $"{_firstName} {_lastName}",
            FirstName = _firstName,
            LastName = _lastName,
            PlayerId = _playerId.Id,
            ShortId = _shortId,
            ReferencePlayerIds = _referencePlayerIds,
            TotalGamesPlayed = _gamesPlayed.Count,
            SeasonTotals = seasonTotals,
            TotalHitting = _totalHitting,
            GamesPlayed = _gamesPlayed.OrderByDescending(x => x.Date).ToList(),
        };
    }

    internal List<Models.SeasonTotal> GetSeasonStats()
    {
        var seasonStats = _gamesPlayed
            .GroupBy(x => x.SeasonId)
            .Select(x => new Models.SeasonTotal
            {
                SeasonId = x.Key,
                SeasonName = x.First().SeasonName,
                GamesPlayed = x.Count(),
                Hitting = x.Select(x => x.Hitting).Aggregate((x, y) => x.AddGame2(y)),
                // Pitching = x.Select(x => x.Pitching).Aggregate((x, y) => x.Add(y)),
                // Fielding = x.Select(x => x.Fielding).Aggregate((x, y) => x.Add(y)),
            })
            .ToList();

        return seasonStats;
    }




    public class GamePlayed
    {
        public required Guid GameId { get; set; }
        public required string ReferenceGameId { get; set; }
        public string? GameShortId { get; set; }
        public string? TeamId { get; set; }
        public string? TeamName { get; set; }
        public string? OppositionId { get; set; }
        public string? OppositionName { get; set; }
        public string? SeasonId { get; set; }
        public string? SeasonName { get; set; }
        public string? Location { get; set; }
        public string? Date { get; set; }
        public string? Result { get; set; }
        public int HomeRuns { get; set; }
        public int AwayRuns { get; set; }

        public Models.HittingData Hitting { get; set; } = new();
        // public PitchingData Pitching { get; set; } = new();
        // public FieldingData Fielding { get; set; } = new();


        public static GamePlayed New(Guid playerId, Models.GameData gameData)
        {
            var player = gameData.Players.FirstOrDefault(x => x.PlayerId == playerId);

            if (player == null)
            {
                throw new Exception($"Player {playerId} not found in game {gameData.GameId}");
            }

            return new GamePlayed
            {
                GameId = gameData.GameId,
                GameShortId = gameData.GameShortId,
                ReferenceGameId = gameData.GameChangerGameId!,
                TeamId = gameData.TeamId,
                TeamName = gameData.TeamName,
                OppositionId = gameData.OppositionId,
                OppositionName = gameData.OppositionName,
                SeasonId = gameData.SeasonId,
                SeasonName = gameData.SeasonName,
                Location = gameData.Location,
                Date = gameData.Date,
                Result = gameData.Result,
                HomeRuns = gameData.HomeRuns,
                AwayRuns = gameData.AwayRuns,
                Hitting = player.Hitting,
            };
        }
    }

}











public class PlayerProfile
{
    public Guid PlayerId { get; set; }
    public List<string> ReferencePlayerIds = new();
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? ShortId { get; set; }


    public List<PlayerProfileBuilder.GamePlayed> GamesPlayed { get; set; } = new();
    public List<Models.SeasonTotal> SeasonTotals { get; set; } = new();
    public Models.HittingData TotalHitting { get; set; } = new();
    public int TotalGamesPlayed { get; set; }
}
