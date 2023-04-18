using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using STKBC.Stats.Pages.Infra;
using STKBC.Stats.Repositories;
using STKBC.Stats.Services;

namespace STKBC.Stats.Pages.GameUploadRequests;

public class UploadModel : PageModel
{
    private readonly IFileUploadService _fileUploadService;
    private readonly IIdGenerator _idGenerator;
    private readonly IClock _clock;
    private readonly IFileService _fileService;
    private readonly IGameUploadRepository _gameUploadRepository;
    private readonly GameChangerImportManager gameChangerImportManager;
    private FileUploadService fileUploadService;

    public UploadModel(
        IFileUploadService fileUploadService,
        IIdGenerator idGenerator,
        IClock clock,
        IFileService fileService,
        IGameUploadRepository gameUploadRepository,
        GameChangerImportManager gameChangerImportManager
        )
    {
        this._fileUploadService = fileUploadService;
        this._idGenerator = idGenerator;
        this._clock = clock;
        this._fileService = fileService;
        this._gameUploadRepository = gameUploadRepository;
        this.gameChangerImportManager = gameChangerImportManager;
    }

    public IActionResult OnGet()
    {

        FileId = FileId ?? Guid.NewGuid();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return RedirectToPage("./Upload").WithModelStateOf(this);
        }

        using var fileUploadStream = GameFile!.OpenReadStream();
        await _fileUploadService.UploadFileAsync(new FileUploadModel
        {
            FileId = FileId,
            FileContentType = GameFile!.ContentType,
            FileName = GameFile!.FileName,
            FileSize = (int)GameFile!.Length
        }, fileUploadStream);

        if (FileType == GameType.GameChanger)
        {
            var gameUpload = await gameChangerImportManager.CreateImportRequestFromFileId(FileId!.Value);

            await _gameUploadRepository.AddGameUploadAsync(gameUpload);
        }
        else
        {
            throw new NotImplementedException();
        }




        return RedirectToPage("./Index");
    }

    [BindProperty]
    public Guid? FileId { get; set; }

    [BindProperty]
    public IFormFile? GameFile { get; set; }

    [BindProperty]
    public GameType? FileType { get; set; }

}

public enum GameType
{
    GameChanger = 1
}