using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using STKBC.Stats.Data.Models;
using STKBC.Stats.Pages.Infra;
using STKBC.Stats.Repositories;

namespace STKBC.Stats.Pages.Players;


public class NewModel : PageModel
{
    private readonly IClubRepository _clubRepository;

    public NewModel(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }

    public IActionResult OnGet()
    {

        BindPropertiesFromState();

        Id = Id ?? Guid.NewGuid();


        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return RedirectToPage("./New").WithModelStateOf(this);
        }

        // await _clubRepository.CreateClubAsync(new Club
        // {
        //     Id = Id!.Value,
        //     Name = Name!,
        //     ShortName = ShortName!,
        //     Key = Key!
        // });

        return RedirectToPage("./Index");
    }


    internal void BindPropertiesFromState()
    {
        var (modelState, exists) = TempData.GetModelState();

        if (!exists)
            return;

        Id = modelState.GetValue<Guid?>(nameof(Id));
        Name = modelState.GetValue<string?>(nameof(Name));
        ShortName = modelState.GetValue<string?>(nameof(ShortName));
        Key = modelState.GetValue<string?>(nameof(Key));


        ModelState.Merge(modelState);
    }

    [BindProperty]
    public string? Name { get; set; }

    [BindProperty]
    public string? ShortName { get; set; }

    [BindProperty]
    public string? Key { get; set; }

    [BindProperty]
    public Guid? Id { get; set; }
}