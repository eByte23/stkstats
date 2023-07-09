using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using STKBC.Stats.Repositories;

namespace STKBC.Stats.Pages.Clubs;


public class IndexModel : PageModel
{

    private readonly IClubRepository _clubRepository;

    public IndexModel(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Clubs = (await _clubRepository.GetClubsAsync()).Select(x => new ClubViewModel
        {
            Id = x.Id,
            Name = x.Name,
            ShortName = x.ShortName,
            Key = x.Key
        }).ToList();


        return Page();
    }

    public List<ClubViewModel> Clubs { get; set; } = new();

    public class ClubViewModel
    {
        public Guid? Id { get; set; }
        public string? Key { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
    }
}