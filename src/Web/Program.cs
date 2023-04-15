using STKBC.Stats.Repositories;
using STKBC.Stats.Services;
using STKBC.Stats.Services.FileStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
            .AddRazorPages()
            .AddMvcOptions(options =>
            {
                // options.Filters.Add<SerializeModelStateFilter>();
            });





var uniqueIdGenerator = new UniqueIdGenerator();

#region InMemory Repos Setup

var gameRepository = new InMemoryGameRepository(new List<STKBC.Stats.Data.Models.Game>
{
    new STKBC.Stats.Data.Models.Game{
        Id = uniqueIdGenerator.NewDeterministicId("game1").Id,
        HomeTeamId = uniqueIdGenerator.NewDeterministicId("home-team-1").Id,
        HomeTeam = "Home Team 1",
        HomeTeamRuns = 10,
        AwayTeamId = uniqueIdGenerator.NewDeterministicId("away-team-2").Id,
        AwayTeam = "Away Team 2",
        AwayTeamRuns = 5,
        LeagueId = uniqueIdGenerator.NewDeterministicId("league-1").Id,
        SeasonId = uniqueIdGenerator.NewDeterministicId("season-1").Id,
        GradeId = uniqueIdGenerator.NewDeterministicId("grade-1").Id,
        GameDate = new DateTime(2023, 5, 4)
    }
});

var seasonRepository = new InMemorySeasonRepository(new List<STKBC.Stats.Data.Models.Season>
{
    new STKBC.Stats.Data.Models.Season{
        Id = uniqueIdGenerator.NewDeterministicId("season-1").Id,
        Name = "2023"
    },
    new STKBC.Stats.Data.Models.Season{
        Id = uniqueIdGenerator.NewDeterministicId("season-2").Id,
        Name = "2022"
    },
});

var leagueRepository = new InMemoryLeagueRepository(new List<STKBC.Stats.Data.Models.League>
{
    new STKBC.Stats.Data.Models.League{
        Id = uniqueIdGenerator.NewDeterministicId("league-1").Id,
        Name = "Super League"
    },
    new STKBC.Stats.Data.Models.League{
        Id = uniqueIdGenerator.NewDeterministicId("league-2").Id,
        Name = "League 2"
    },
});

var gradesRepository = new InMemoryGradeRepository(new List<STKBC.Stats.Data.Models.Grade>
{
    new STKBC.Stats.Data.Models.Grade{
        Id = uniqueIdGenerator.NewDeterministicId("grade-1").Id,
        Name = "A Grade"
    },
    new STKBC.Stats.Data.Models.Grade{
        Id = uniqueIdGenerator.NewDeterministicId("grade-2").Id,
        Name = "B Grade"
    },
    new STKBC.Stats.Data.Models.Grade{
        Id = uniqueIdGenerator.NewDeterministicId("grade-3").Id,
        Name = "C Grade"
    }
});

var gameUploadRepository = new InMemoryGameUploadRepository();
var fileUploadRepository = new InMemoryFileUploadRepository();

#endregion


builder.Services.AddScoped<IGameRepository>(s => gameRepository);
builder.Services.AddScoped<ISeasonRepository>(s => seasonRepository);
builder.Services.AddScoped<ILeagueRepository>(s => leagueRepository);
builder.Services.AddScoped<IGradeRepository>(s => gradesRepository);
builder.Services.AddScoped<IIdGenerator>(s => uniqueIdGenerator);
builder.Services.AddScoped<IGameUploadRepository>(s => gameUploadRepository);
builder.Services.AddScoped<IFileUploadRepository>(s => fileUploadRepository);

builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IClock, Clock>();
builder.Services.AddScoped<IFileStore>(s=> new FileSystemFileStore("/Users/elijahbate/Personal/Dev/stats/.temp-files"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
