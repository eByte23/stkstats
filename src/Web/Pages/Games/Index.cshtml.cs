using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using STKBC.Stats.Repositories;

namespace STKBC.Stats.Pages.Games;

public class IndexModel : PageModel
{
    private readonly IGameRepository _gameRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly ILeagueRepository _leagueRepository;
    private readonly IGradeRepository _gradesRepository;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        IGameRepository gameRepository,
        ISeasonRepository seasonRepository,
        ILeagueRepository leagueRepository,
        IGradeRepository gradesRepository,
        ILogger<IndexModel> logger
        )
    {
        _gameRepository = gameRepository;
        _seasonRepository = seasonRepository;
        _leagueRepository = leagueRepository;
        _gradesRepository = gradesRepository;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        Games = GetGamesViews();

        PendingGameImports = 4;

        return Page();
    }

    public List<GameView> Games { get; set; } = new();

    public int? PendingGameImports { get; set; }


    internal List<GameView> GetGamesViews()
    {
        var grades = _gradesRepository.GetGrades();
        var leagues = _leagueRepository.GetLeagues();
        var seasons = _seasonRepository.GetSeasons();

        return _gameRepository.GetGames().Select(game =>
        {
            var grade = grades.Single(g => g.Id == game.GradeId);
            var league = leagues.Single(l => l.Id == game.LeagueId);
            var season = seasons.Single(s => s.Id == game.SeasonId);

            var winnerName = game.HomeTeamRuns > game.AwayTeamRuns ? game.HomeTeam : game.AwayTeam;

            var gameView = new GameView
            {
                GameId = game.Id,
                HomeTeamId = game.HomeTeamId,
                HomeTeamName = game.HomeTeam,
                AwayTeamId = game.AwayTeamId,
                AwayTeamName = game.AwayTeam,
                GameDate = game.GameDate!.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                GradeName = grade.Name,
                LeagueName = league!.Name,
                SeasonName = season!.Name,
                Result = $"{winnerName} wins {game.HomeTeamRuns}-{game.AwayTeamRuns}"
            };

            return gameView;
        }).ToList();
    }
}

public class GameView
{
    public Guid? GameId { get; set; }
    public string? HomeTeamName { get; set; }
    public string? AwayTeamName { get; set; }
    public string? GameDate { get; set; }
    public string? GradeName { get; set; }
    public string? LeagueName { get; set; }
    public string? SeasonName { get; set; }
    public string? Result { get; set; }
    public Guid? AwayTeamId { get; set; }
    public Guid? HomeTeamId { get; set; }
}