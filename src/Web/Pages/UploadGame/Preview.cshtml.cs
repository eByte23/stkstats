using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using STKBC.Stats.Pages.Models.Partials;

namespace STKBC.Stats.Pages.UploadGame;

public class PreviewModel : PageModel
{
    private readonly ILogger<PreviewModel> _logger;

    private static GamePreview _game;

    static PreviewModel()
    {
        _game = PreviewModel.GetGamePreview(Guid.Empty);
    }

    public PreviewModel(ILogger<PreviewModel> logger)
    {
        _logger = logger;
    }


    public void OnGet()
    {
        Game = _game;
    }

    public IActionResult OnPostCreateAndLinkPlayer([FromForm] Guid? PlayerTempId, [FromForm] Guid? TeamId, [FromForm] string PlayerName)
    {
        var playerId = Guid.NewGuid();



        var team = _game.HomeTeam!.TeamId == TeamId ? _game.HomeTeam : _game.AwayTeam;

        var player = team!.Players.FirstOrDefault(x => x.TempId == PlayerTempId);
        if (player != null)
        {
            player.Matched = true;
            player.MatchedId = playerId;
        }

        SaveGamePreview(_game);

        return RedirectToPage("./Preview", new { GameId = GameId });
    }


    public IActionResult OnPostLinkPlayer([FromForm] Guid? PlayerId, [FromForm] Guid? PlayerTempId, [FromForm] Guid? TeamId)
    {
        var team = _game.HomeTeam!.TeamId == TeamId ? _game.HomeTeam : _game.AwayTeam;

        var player = team!.Players.FirstOrDefault(x => x.TempId == PlayerTempId);
        if (player != null)
        {
            player.Matched = true;
            player.MatchedId = PlayerId;
        }

        SaveGamePreview(_game);

        return RedirectToPage("./Preview", new { GameId = GameId });
    }

    public IActionResult OnPost()
    {
        

        return RedirectToPage("../Games/Index");
    }

    [FromRoute]
    public Guid GameId { get; set; }

    public GamePreview? Game { get; set; }


    private void SaveGamePreview(GamePreview game)
    {
        throw new NotImplementedException();
    }



    private static GamePreview GetGamePreview(Guid? id)
    {

        return new GamePreview
        {
            GameId = Guid.NewGuid(),
            GradeId = Guid.NewGuid(),
            HomeTeam = new TeamPreview
            {
                TeamId = new Guid("4f204b2d-9d0b-4512-9143-afb7f2e0aadc"),
                TeamName = "St Kilda B Reserve",
                Players = new List<PlayerPreview>{
                    new PlayerPreview{
                        TempId = new Guid("7a7e3250-c17f-4442-83a0-327356b6d71a"),
                        DisplayName = "Elijah Bate",
                        Hitting = new HittingStatPreview{
                            AB=3,
                            AVG=333,
                            Doubles=1,
                            H=0,
                            HR = 0,
                            OPS = 10,
                            Runs = 1,
                            RBI =2,
                            SO = 0,
                            Triples = 0,
                            BB = 0,
                            CS = 0,
                            OBP = 0,
                            SB = 1,
                            SLG = 222
                        }
                    }
                }
            },
            AwayTeam = new TeamPreview
            {
                TeamId = new Guid("26527378-949d-4e6e-a291-c3e7b362dbd0"),
                TeamName = "Doncaster",
                Players = new List<PlayerPreview>
                {
                     new PlayerPreview{
                        TempId = new Guid("a70d3305-6692-4053-9287-799f7ce84af2"),
                        DisplayName = "Alan Ortiz",
                        Hitting = new HittingStatPreview{
                            AB=3,
                            AVG=333,
                            Doubles=1,
                            H=0,
                            HR = 0,
                            OPS = 10,
                            Runs = 1,
                            RBI =2,
                            SO = 0,
                            Triples = 0,
                            BB = 0,
                            CS = 0,
                            OBP = 0,
                            SB = 1,
                            SLG = 222
                        }
                    }
                }
            },
        };
    }
}