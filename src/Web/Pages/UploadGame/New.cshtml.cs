using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using STKBC.Stats.Data.Models;
using STKBC.Stats.Pages.Infra;
using STKBC.Stats.Repositories;
using STKBC.Stats.Services;

namespace STKBC.Stats.Pages.UploadGame;

public class NewModel : PageModel
{
    private readonly IGameUploadRepository _gameUploadRepository;
    private readonly IGamePreviewRepository _gamePreviewRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly ILeagueRepository _leagueRepository;
    private readonly IGradeRepository _gradeRepository;
    private readonly IClubRepository _clubRepository;
    private readonly GameChangerImportManager gameChangerImportManager;
    private readonly ILogger<NewModel> _logger;

    public NewModel(
        IGameUploadRepository gameUploadRepository,
        IGamePreviewRepository gamePreviewRepository,
        ISeasonRepository seasonRepository,
        ILeagueRepository leagueRepository,
        IGradeRepository gradeRepository,
        IClubRepository clubRepository,
        GameChangerImportManager gameChangerImportManager,
        ILogger<NewModel> logger
        )
    {
        _gameUploadRepository = gameUploadRepository;
        _gamePreviewRepository = gamePreviewRepository;
        _seasonRepository = seasonRepository;
        _leagueRepository = leagueRepository;
        _gradeRepository = gradeRepository;
        _clubRepository = clubRepository;
        this.gameChangerImportManager = gameChangerImportManager;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        BindPropertiesFromState();

        ImportRequestId = ImportRequestId; // ?? Guid.NewGuid();

        var gameUpload = await _gameUploadRepository.GetGameUploadAsync(ImportRequestId!.Value);

        ImportRequest = new ImportRequestViewModel
        {
            FileName = gameUpload!.FileName,
            AwayTeam = gameUpload!.AwayTeam,
            HomeTeam = gameUpload!.HomeTeam,
        };

        Leagues = MapLeaguesToSelectList(_leagueRepository.GetLeagues(), LeagueId);

        var clubs = await _clubRepository.GetClubsAsync();

        HomeTeams = MapTeamsToSelectList(clubs, HomeTeamId);

        AwayTeams = MapTeamsToSelectList(clubs, AwayTeamId);

        Grades = MapGradesToSelectList(_gradeRepository.GetGrades(), GradeId);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return RedirectToPage("./New").WithModelStateOf(this);
        }

        var gameUpload = await _gameUploadRepository.GetGameUploadAsync(ImportRequestId!.Value);
        var a = await gameChangerImportManager.GetTemporaryGameUploadFromImportRequest(gameUpload);

        Guid? id = a.Id;
        await _gamePreviewRepository.CreateGamePreviewAsync(new GamePreview
        {
            GameId = id,
            GradeId = GradeId,
            LeagueId = LeagueId,
            AwayTeam = new TeamPreview
            {
                TeamId = AwayTeamId,
                TeamName = a.AwayTeam.Name,
                Players = MapToPlayers(a.AwayTeam)

            },
            HomeTeam = new TeamPreview
            {
                TeamId = HomeTeamId,
                TeamName = a.HomeTeam.Name,
                Players = MapToPlayers(a.HomeTeam)

            },
        });

        gameUpload.GamePreviewId = id;

        await _gameUploadRepository.UpdateGameUploadAsync(gameUpload);

        return RedirectToPage("./Preview", new { GameId = id });
    }

    private static List<PlayerPreview> MapToPlayers(TemporaryTeam a)
    {
        return a.Players.Select(x => new PlayerPreview
        {
            DisplayName = x.DisplayName,
            Hitting = new HittingStatPreview
            {
                AB = x.Batting.Ab,
                BB = x.Batting.Bb,
                CS = x.Batting.Cs,
                // AVG = x.Batting.,
                // HBP = x.Batting.Hbp,
                H = x.Batting.H,
                HR = x.Batting.Hr,
                RBI = x.Batting.Rbi,
                Runs = x.Batting.R,
                SB = x.Batting.Sb,
                // SF = x.Batting.Sf,
                // SLG = x.Batting.,
                SO = x.Batting.So,

                // OBP = x.Batting.Obp,
                // OPS = x.Batting.Ops,
                Doubles = x.Batting.Double,
                Triples = x.Batting.Triple,
            },
            Matched = x.Found,
            MatchedId = null,
            TempId = x.PlayerId,


        }).ToList();
    }

    internal void BindPropertiesFromState()
    {
        var (modelState, exists) = TempData.GetModelState();

        if (!exists)
            return;

        ImportRequestId = modelState.GetValue<Guid?>(nameof(ImportRequestId));
        LeagueId = modelState.GetValue<Guid?>(nameof(LeagueId));
        GradeId = modelState.GetValue<Guid?>(nameof(GradeId));
        HomeTeamId = modelState.GetValue<Guid?>(nameof(HomeTeamId));
        AwayTeamId = modelState.GetValue<Guid?>(nameof(AwayTeamId));

        ModelState.Merge(modelState);
    }

    public class ImportRequestViewModel
    {
        public string? FileName { get; set; }
        public string? HomeTeam { get; set; }
        public string? AwayTeam { get; set; }
    }

    [Required]
    [FromQuery]
    public Guid? ImportRequestId { get; set; }

    public ImportRequestViewModel? ImportRequest { get; set; }


    public List<SelectListItem>? Leagues { get; set; }

    [Required]
    [BindProperty]
    public Guid? LeagueId { get; set; }

    public List<SelectListItem>? Grades { get; set; }

    [Required]
    [BindProperty]
    public Guid? GradeId { get; set; }

    public List<SelectListItem>? HomeTeams { get; set; }

    [Required]
    [BindProperty]
    public Guid? HomeTeamId { get; set; }

    public List<SelectListItem>? AwayTeams { get; set; }

    [Required]
    [BindProperty]
    public Guid? AwayTeamId { get; set; }


    // public class League
    // {
    //     public Guid? Id { get; set; }
    //     public string? Name { get; set; }
    // }

    // public class Grade
    // {
    //     public Guid? Id { get; set; }
    //     public string? Name { get; set; }
    // }

    // public class Team
    // {
    //     public Guid? Id { get; set; }
    //     public string? Name { get; set; }
    // }



    internal List<SelectListItem> MapLeaguesToSelectList(List<League> leagues, Guid? selectedLeagueId)
    {
        var listItems = new List<SelectListItem>(){
            new SelectListItem("Select League", "", !selectedLeagueId.HasValue, false)
        };

        listItems.AddRange(leagues.Select(l => new SelectListItem(l.Name!, l.Id!.Value.ToString(), l.Id == selectedLeagueId)));

        return listItems;
    }

    internal List<SelectListItem> MapTeamsToSelectList(List<Club> Teams, Guid? selectedTeamId)
    {
        var listItems = new List<SelectListItem>(){
            new SelectListItem("Select Team", "", !selectedTeamId.HasValue, false)
        };

        listItems.AddRange(Teams.Select(t => new SelectListItem(t.Name!, t.Id!.Value.ToString(), t.Id == selectedTeamId)));
        return listItems;
    }

    internal List<SelectListItem> MapGradesToSelectList(List<Grade> Grades, Guid? gradeId)
    {
        var listItems = new List<SelectListItem>(){
            new SelectListItem("Select Grade", "", !gradeId.HasValue , false)
        };

        listItems.AddRange(Grades.Select(g => new SelectListItem(g.Name!, g.Id!.Value.ToString(), g.Id == gradeId)));
        return listItems;
    }
}
