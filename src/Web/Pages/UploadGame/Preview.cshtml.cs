using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using STKBC.Stats.Data.Models;
using STKBC.Stats.Pages.Models.Partials;
using STKBC.Stats.Repositories;

namespace STKBC.Stats.Pages.UploadGame;

public class PreviewModel : PageModel
{
    private readonly IGamePreviewRepository _gamePreviewRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger<PreviewModel> _logger;

    public PreviewModel(
        IGamePreviewRepository gamePreviewRepository,
        IPlayerRepository playerRepository,
        ILogger<PreviewModel> logger
    )
    {
        _gamePreviewRepository = gamePreviewRepository;
        _playerRepository = playerRepository;
        _logger = logger;
    }



    public async Task<IActionResult> OnGetAsync()
    {
        Game = await GetGamePreviewAsync(GameId);

        return Page();
    }

    public async Task<IActionResult> OnPostCreateAndLinkPlayerAsync([FromForm] Guid? PlayerTempId, [FromForm] Guid? TeamId, [FromForm] string PlayerName)
    {
        var playerId = Guid.NewGuid();


        var game = await _gamePreviewRepository.GetGamePreviewAsync(GameId);


        var team = game.HomeTeam!.TeamId == TeamId ? game.HomeTeam : game.AwayTeam;

        var player = team!.Players.FirstOrDefault(x => x.TempId == PlayerTempId);
        if (player != null)
        {

            await _playerRepository.CreatePlayerAsync(new Player
            {
                Id = playerId,
                DisplayName = PlayerName,
                FirstName = PlayerName,
                LastName = PlayerName,
                BirthDate = DateTime.Now
            });


            player.Matched = true;
            player.MatchedId = playerId;
        }

        await _gamePreviewRepository.UpdateGamePreviewAsync(game);

        return RedirectToPage("./Preview", new { GameId = GameId });
    }


    public async Task<IActionResult> OnPostLinkPlayerAsync([FromForm] Guid? PlayerId, [FromForm] Guid? PlayerTempId, [FromForm] Guid? TeamId)
    {
        var game = await _gamePreviewRepository.GetGamePreviewAsync(GameId);

        var team = game.HomeTeam!.TeamId == TeamId ? game.HomeTeam : game.AwayTeam;

        var player = team!.Players.FirstOrDefault(x => x.TempId == PlayerTempId);
        if (player != null)
        {
            player.Matched = true;
            player.MatchedId = PlayerId;
        }

        await _gamePreviewRepository.UpdateGamePreviewAsync(game);

        return RedirectToPage("./Preview", new { GameId = GameId });
    }

    public IActionResult OnPost()
    {


        return RedirectToPage("../Games/Index");
    }

    [FromQuery]
    public Guid GameId { get; set; }

    public GamePreviewView? Game { get; set; }


    private void SaveGamePreview(GamePreviewView game)
    {
        throw new NotImplementedException();
    }



    private async Task<GamePreviewView?> GetGamePreviewAsync(Guid? id)
    {
        if (!id.HasValue)
        {
            return null;
        }

        var gamePreview = await _gamePreviewRepository.GetGamePreviewAsync(id.Value);

        if (gamePreview == null)
        {
            return null;
        }

        return new GamePreviewView
        {
            GameId = gamePreview.GameId,
            GradeId = gamePreview.GradeId,
            HomeTeam = new TeamPreviewView
            {
                GameId = gamePreview.GameId,
                TeamId = gamePreview.HomeTeam?.TeamId,
                TeamName = gamePreview.HomeTeam?.TeamName,
                Players = MapPlayersToPlayerPreviewList(gamePreview.HomeTeam)
            },
            AwayTeam = new TeamPreviewView
            {
                GameId = gamePreview.GameId,
                TeamId = gamePreview.AwayTeam?.TeamId,
                TeamName = gamePreview.AwayTeam?.TeamName,
                Players = MapPlayersToPlayerPreviewList(gamePreview.AwayTeam)
            },

        };
    }

    private static List<PlayerPreviewView> MapPlayersToPlayerPreviewList(TeamPreview? teamPreview)
    {
        if (teamPreview?.Players == null)
        {
            return new List<PlayerPreviewView>();
        }

        return teamPreview.Players.Select(x => new PlayerPreviewView
        {
            TempId = x.TempId,
            DisplayName = x.DisplayName,
            Hitting = new HittingStatPreviewView
            {
                AB = x.Hitting?.AB,
                AVG = x.Hitting?.AVG,
                Doubles = x.Hitting?.Doubles,
                H = x.Hitting?.H,
                HR = x.Hitting?.HR,
                OPS = x.Hitting?.OPS,
                Runs = x.Hitting?.Runs,
                RBI = x.Hitting?.RBI,
                SO = x.Hitting?.SO,
                Triples = x.Hitting?.Triples,
                BB = x.Hitting?.BB,
                CS = x.Hitting?.CS,
                OBP = x.Hitting?.OBP,
                SB = x.Hitting?.SB,
                SLG = x.Hitting?.SLG,
            },
            Matched = x.Matched,
            MatchedId = x.MatchedId
        }).ToList();
    }
}