using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using STKBC.Stats.Pages.Infra;

namespace STKBC.Stats.Pages.UploadGame;

public class NewModel : PageModel
{
    private readonly ILogger<NewModel> _logger;

    public NewModel(ILogger<NewModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        BindPropertiesFromState();

        ImportRequestId = ImportRequestId ?? Guid.NewGuid();

        Leagues = MapLeaguesToSelectList(new List<League>(){
            new League(){ Id = new Guid("1e4c827f-eaae-4ea0-b38e-0ea67c9f4fb8"), Name = "League 1" },
            new League(){ Id = new Guid("5f70df94-e87c-403e-a4d0-7bb83fce2899"), Name = "League 2" },
            new League(){ Id = new Guid("489a1705-25c6-408e-a52c-29d93091e1cf"), Name = "League 3" },
        }, LeagueId);

        HomeTeams = MapTeamsToSelectList(new List<Team>(){
            new Team(){ Id = new Guid("a797916c-d0e3-4b37-82c2-8c1a210928bd"), Name = "Team 1" },
            new Team(){ Id = new Guid("646bfb15-885c-45ec-a16f-a749bf5491fa"), Name = "Team 2" },
            new Team(){ Id = new Guid("e0c76b43-988b-4d75-9106-381eb3ec5c32"), Name = "Team 3" },
        }, HomeTeamId);

        AwayTeams = MapTeamsToSelectList(new List<Team>(){
            new Team(){ Id = new Guid("d0f1a26e-ef2e-4e5a-9513-6efed8776a48"), Name = "Team 1" },
            new Team(){ Id = new Guid("842821f4-15ba-40b6-8ab9-c7c756146f5a"), Name = "Team 2" },
            new Team(){ Id = new Guid("b403e947-be3a-4d42-92b3-71acf08bf516"), Name = "Team 3" },
        }, AwayTeamId);

        Grades = MapGradesToSelectList(new List<Grade>(){
            new Grade(){ Id = new Guid("8b13e8e9-67dd-4c68-8e3e-d2a6a9ed631a"), Name = "Grade 1" },
            new Grade(){ Id = new Guid("d2878855-3f18-4a50-8841-ade16c69c507"), Name = "Grade 2" },
            new Grade(){ Id = new Guid("6d2680d0-3f90-44c8-bb3b-a2c59cd43743"), Name = "Grade 3" },
        }, GradeId);
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return RedirectToPage("./New").WithModelStateOf(this);
        }

        return RedirectToPage("./Preview", new { GameId = Guid.NewGuid() });
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
    [BindProperty]
    public Guid? ImportRequestId { get; set; }

    public ImportRequestViewModel ImportRequest { get; set; } = new ImportRequestViewModel
    {
        FileName = "test.csv",
        HomeTeam = "Team 1",
        AwayTeam = "Team 2"
    };


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


    public class League
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }

    public class Grade
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }

    public class Team
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }



    internal List<SelectListItem> MapLeaguesToSelectList(List<League> leagues, Guid? selectedLeagueId)
    {
        var listItems = new List<SelectListItem>(){
            new SelectListItem("Select League", "", !selectedLeagueId.HasValue, false)
        };

        listItems.AddRange(leagues.Select(l => new SelectListItem(l.Name!, l.Id!.Value.ToString(), l.Id == selectedLeagueId)));

        return listItems;
    }

    internal List<SelectListItem> MapTeamsToSelectList(List<Team> Teams, Guid? selectedTeamId)
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
