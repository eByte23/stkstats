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
});

var seasonRepository = new InMemorySeasonRepository(new List<STKBC.Stats.Data.Models.Season>
{
    new STKBC.Stats.Data.Models.Season{
        Id = uniqueIdGenerator.NewDeterministicId("season-2023").Id,
        Name = "2023"
    },
    new STKBC.Stats.Data.Models.Season{
        Id = uniqueIdGenerator.NewDeterministicId("season-2022").Id,
        Name = "2022"
    },
    new STKBC.Stats.Data.Models.Season{
        Id = uniqueIdGenerator.NewDeterministicId("season-2021").Id,
        Name = "2021"
    },
    new STKBC.Stats.Data.Models.Season{
        Id = uniqueIdGenerator.NewDeterministicId("season-2020").Id,
        Name = "2020"
    },
    new STKBC.Stats.Data.Models.Season{
        Id = uniqueIdGenerator.NewDeterministicId("season-2019").Id,
        Name = "2019"
    },
    new STKBC.Stats.Data.Models.Season{
        Id = uniqueIdGenerator.NewDeterministicId("season-2018").Id,
        Name = "2018"
    },
});

var leagueRepository = new InMemoryLeagueRepository(new List<STKBC.Stats.Data.Models.League>
{
    new STKBC.Stats.Data.Models.League{
        Id = uniqueIdGenerator.NewDeterministicId("vsbl").Id,
        Name = "Victoria Summer Baseball League",
        Key = "vsbl",
        ShortName = "Vic Summer"
    },
    new STKBC.Stats.Data.Models.League{
        Id = uniqueIdGenerator.NewDeterministicId("mwbl").Id,
        Name = "Melbourne Winter Baseball League",
        Key = "mwbl",
        ShortName = "Melbourne Winter"
    },
});

var gradesRepository = new InMemoryGradeRepository(new List<STKBC.Stats.Data.Models.Grade>
{
    new STKBC.Stats.Data.Models.Grade{
        Id = uniqueIdGenerator.NewDeterministicId("grade-a").Id,
        Name = "A Grade",
        LeagueId = uniqueIdGenerator.NewDeterministicId("mwbl").Id
    },
    new STKBC.Stats.Data.Models.Grade{
        Id = uniqueIdGenerator.NewDeterministicId("grade-a-reserve").Id,
        Name = "A Grade Reserve",
        LeagueId = uniqueIdGenerator.NewDeterministicId("mwbl").Id
    },
    new STKBC.Stats.Data.Models.Grade{
        Id = uniqueIdGenerator.NewDeterministicId("grade-b").Id,
        Name = "B Grade",
        LeagueId = uniqueIdGenerator.NewDeterministicId("mwbl").Id
    },
    new STKBC.Stats.Data.Models.Grade{
        Id = uniqueIdGenerator.NewDeterministicId("grade-b-reserve").Id,
        Name = "B Grade Reserve",
        LeagueId = uniqueIdGenerator.NewDeterministicId("mwbl").Id
    },
    new STKBC.Stats.Data.Models.Grade{
        Id = uniqueIdGenerator.NewDeterministicId("grade-c").Id,
        Name = "C Grade",
        LeagueId = uniqueIdGenerator.NewDeterministicId("mwbl").Id
    },
    new STKBC.Stats.Data.Models.Grade{
        Id = uniqueIdGenerator.NewDeterministicId("grade-c-reserve").Id,
        Name = "C Grade Reserve",
        LeagueId = uniqueIdGenerator.NewDeterministicId("mwbl").Id
    }
});

var clubRepository = new InMemoryClubRepository(new List<STKBC.Stats.Data.Models.Club>
{
    new STKBC.Stats.Data.Models.Club{
        Id = uniqueIdGenerator.NewDeterministicId("stkbc").Id,
        Name = "St Kilda Baseball Club",
        Key="stkbc",
        ShortName = "St Kilda",
    },
    new STKBC.Stats.Data.Models.Club{
        Id = uniqueIdGenerator.NewDeterministicId("dncs").Id,
        Name = "Doncaster Baseball Club",
        Key="dncs",
        ShortName = "Doncaster",
    },
    new STKBC.Stats.Data.Models.Club{
        Id = uniqueIdGenerator.NewDeterministicId("crdn").Id,
        Name = "Croydon Baseball Club",
        Key="crdn",
        ShortName = "Croydon",
    },
    new STKBC.Stats.Data.Models.Club{
        Id = uniqueIdGenerator.NewDeterministicId("esdn").Id,
        Name = "Essendon Baseball Club",
        Key="esdn",
        ShortName = "Essendon",
    },
     new STKBC.Stats.Data.Models.Club{
        Id = uniqueIdGenerator.NewDeterministicId("wvrl").Id,
        Name = "Waverley Baseball Club",
        Key="wvrl",
        ShortName = "Waverley",
    },
    new STKBC.Stats.Data.Models.Club{
        Id = uniqueIdGenerator.NewDeterministicId("wlrhil").Id,
        Name = "Wheelers Hill Baseball Club",
        Key="wlrhil",
        ShortName = "Wheelers Hill",
    },
    new STKBC.Stats.Data.Models.Club{
        Id = uniqueIdGenerator.NewDeterministicId("pkhn").Id,
        Name = "Pakenham Baseball Club",
        Key="pkhn",
        ShortName = "Pakenham",
    },
});

var gameUploadRepository = new InMemoryGameUploadRepository();
var fileUploadRepository = new InMemoryFileUploadRepository();
var gamePreviewRepository = new InMemoryGamePreviewRepository();
var playerRepository = new InMemoryPlayerRepository();

#endregion

builder.Services.AddScoped<GameChangerImportManager>();
builder.Services.AddScoped<IPlayerRepository,LocalStorageFilePlayerRepository>();
// builder.Services.AddScoped<IPlayerRepository>(s => playerRepository);
// builder.Services.AddScoped<IClubRepository>(s => clubRepository);
builder.Services.AddScoped<IClubRepository, LocalStorageClubRepository>();
builder.Services.AddScoped<IGameRepository>(s => gameRepository);
builder.Services.AddScoped<IGamePreviewRepository, LocalStorageFileGamePreviewRepository>();
// builder.Services.AddScoped<IGamePreviewRepository>(s => gamePreviewRepository);
builder.Services.AddScoped<ISeasonRepository>(s => seasonRepository);
builder.Services.AddScoped<ILeagueRepository>(s => leagueRepository);
builder.Services.AddScoped<IGradeRepository>(s => gradesRepository);
builder.Services.AddScoped<IIdGenerator>(s => uniqueIdGenerator);
// builder.Services.AddScoped<IGameUploadRepository>(s => gameUploadRepository);
builder.Services.AddScoped<IGameUploadRepository, LocalStorageFileGameUploadRepository>();
builder.Services.AddScoped<IFileUploadRepository, LocalStorageFileUploadRepository>();
// builder.Services.AddScoped<IFileUploadRepository>(s => fileUploadRepository);
builder.Services.AddScoped<RepoFileSystemStorageHelper>(s => new RepoFileSystemStorageHelper("/Users/elijahbate/Personal/Dev/stats/.temp-files"));


builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IClock, Clock>();
builder.Services.AddScoped<IFileStore>(s => new FileSystemFileStore("/Users/elijahbate/Personal/Dev/stats/.temp-files"));



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
