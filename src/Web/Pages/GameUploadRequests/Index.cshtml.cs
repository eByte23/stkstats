using Microsoft.AspNetCore.Mvc.RazorPages;
using STKBC.Stats.Data.Models;
using STKBC.Stats.Repositories;

namespace STKBC.Stats.Pages.GameUploadRequests;

public class IndexModel : PageModel
{
    private readonly IGameUploadRepository _gameUploadRepository;
    public IndexModel(IGameUploadRepository gameUploadRepository)
    {
        _gameUploadRepository = gameUploadRepository;
    }

    public async Task OnGetAsync()
    {
        GameUploads = await _gameUploadRepository.GetGameUploadsAsync();
    }


    public List<GameUpload> GameUploads { get; set; } = new();
}