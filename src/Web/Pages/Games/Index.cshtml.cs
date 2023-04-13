using Microsoft.AspNetCore.Mvc.RazorPages;

namespace STKBC.Stats.Pages.Games;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public List<GameView> Games { get; set; } = new();
}

public class GameView
{
    public Guid? GameId { get; set; }
    public string? HomeTeamName { get; set; }
    public string? AwayTeamName { get; set; }
    public DateTime? GameDate { get; set; }
    public string? GradeName { get; set; }
    public string? LeagueName { get; set; }
    public string? Result { get; set; }
}