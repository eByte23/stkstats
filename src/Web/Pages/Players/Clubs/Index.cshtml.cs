using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using STKBC.Stats.Repositories;

namespace STKBC.Stats.Pages.Players;


public class IndexModel : PageModel
{

    private readonly IClubRepository _clubRepository;
    private readonly IPlayerRepository _playerRepository;

    public IndexModel(
        IClubRepository clubRepository,
        IPlayerRepository playerRepository
        )
    {
        _clubRepository = clubRepository;
        _playerRepository = playerRepository;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var players = await _playerRepository.GetPlayersAsync();
        var clubs = await _clubRepository.GetClubsAsync();


        Players = players!.Select(x => new PlayerViewModel
        {
            Id = x.Id,
            ClubId = x.CurrentClubId,
            DisplayName = x.DisplayName,
            PrimaryPosition = x.PrimaryPosition
        }).ToList();

        Clubs = clubs!.Select(x => new ClubViewModel
        {
            Id = x.Id,
            Name = x.Name
        }).ToDictionary(x => x.Id!.Value, x => x);


        return Page();
    }

    public Dictionary<Guid, ClubViewModel> Clubs { get; set; } = new();
    public List<PlayerViewModel> Players { get; set; } = new();

    public class PlayerViewModel
    {
        public Guid? Id { get; set; }
        public string? DisplayName { get; set; }
        public string? PrimaryPosition { get; set; }
        public Guid? ClubId { get; set; }
    }

    public class ClubViewModel
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }
    }
}