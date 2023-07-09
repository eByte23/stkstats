using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace STKBC.Stats.Pages.Clubs;


public class ViewModel : PageModel
{
    public ViewModel()
    {
    }

    [FromRoute]
    public Guid? ClubId { get; set; }
    
    public IActionResult OnGet()
    {



        return Page();
    }
    public IActionResult OnPost()
    {



        return Page();
    }
}