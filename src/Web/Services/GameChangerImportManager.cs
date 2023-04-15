using System.Globalization;
using System.Xml;
using STKBC.Stats.Data.Models;
using STKBC.Stats.Parser;

namespace STKBC.Stats.Services;


public class GameChangerImportManager
{
    private IIdGenerator _idGenerator;
    private IClock _clock;
    private IFileService _fileService;

    public GameChangerImportManager(IIdGenerator idGenerator, IClock clock, IFileService fileService)
    {
        this._idGenerator = idGenerator;
        this._clock = clock;
        this._fileService = fileService;
    }

    public async Task<GameUpload> CreateImportRequestFromFileId(Guid? fileId)
    {
        if (!fileId.HasValue)
            throw new ArgumentNullException(nameof(fileId));

        var file = await _fileService.GetFileObjectAsync(fileId);

        if (file is null)
            throw new Exception($"File with Id: `{fileId}` could not be found");


        var fileObjectStream = await _fileService.GetFileObjectStream(file.Id);
        var fileXml = fileObjectStream.GetText();

        var peeker = new GameChangerXmlPeeker();
        var overview = peeker.GetFileOverviewFromXml(fileXml);

        return MapToFileImportRequest(file, overview, _idGenerator, _clock);
    }

    internal static GameUpload MapToFileImportRequest(
        FileObject file,
        GameChangerFileOverview overview,
        IIdGenerator idGenerator,
        IClock clock
        )
    {


        return new GameUpload
        {
            Id = idGenerator.NewDeterministicId(file.Id!.Value, "import-request-id").Id,
            FileId = file.Id,
            FileName = file.Name,
            FileHash = file.Hash,
            HomeTeam = overview.HomeTeam,
            AwayTeam = overview.AwayTeam,
            ExternalRef = overview.GameId,
            GameDate = overview.GameDate,
            UploadedAt = clock.GetUtcNow(),
            ImportType = $"GameChanger_{overview.Format}"
        };
    }

    public async Task<TemporaryGameUpload> GetTemporaryGameUploadFromImportRequest(GameUpload importRequest)
    {


        var fileObjectStream = await _fileService.GetFileObjectStream(importRequest.FileId);
        var fileXml = fileObjectStream.GetText();

        var parsedGame = Stats.Parser.ParserUtil.Deserialize(fileXml);
        var tempGameId = _idGenerator.NewDeterministicId(importRequest.Id!.Value, "temp-game-upload");

        var homeTeamName = parsedGame.Venue.Homename;
        var awayTeamName = parsedGame.Venue.Visname;

        var homeTeam = parsedGame.Teams.Single(x => x.Name == homeTeamName);
        var awayTeam = parsedGame.Teams.Single(x => x.Name == awayTeamName);

        var temporaryTeamHome = MapTemporaryTeam(tempGameId, homeTeam);
        var temporaryTeamAway = MapTemporaryTeam(tempGameId, awayTeam);

        return new TemporaryGameUpload
        {
            Id = tempGameId.Id,
            ImportRequestId = importRequest.Id,
            HomeTeam = temporaryTeamHome,
            AwayTeam = temporaryTeamAway,
            CreatedAt = _clock.GetUtcNow(),
            FileId = importRequest.FileId,
            FileName = importRequest.FileName,
            GameDate = importRequest.GameDate,
            GradeId = null,
            LeagueId = null,
        };
    }


    private TemporaryTeam MapTemporaryTeam(DeterministicGuid tempGameId, GameChanger.Team team)
    {
        var tempTeamId = tempGameId.NewGuid(team.Name);
        return new TemporaryTeam
        {
            Id = tempTeamId.Id,
            Name = team.Name,
            Players = MapToTemporaryPlayers(team, tempTeamId),
            Found = false,
        };
    }

    private List<TemporaryPlayer> MapToTemporaryPlayers(GameChanger.Team team, DeterministicGuid tempTeamId)
    {
        return team.Players.Select((x, idx) => new TemporaryPlayer
        {
            PlayerId = tempTeamId.NewGuid(x.Name).Id,
            DisplayName = x.Name,
            Pitching = null,
            Batting = new TemporaryPlayerBattingStats
            {
                Ab = x.Hitting.Ab,
                Bb = x.Hitting.Bb,
                Cs = x.Hitting.Cs,
                Double = x.Hitting.Double,
                Gdp = x.Hitting.Gdp,
                Ground = x.Hitting.Ground,
                H = x.Hitting.H,
                Hbp = x.Hitting.Hbp,
                Hr = x.Hitting.Hr,
                Kl = x.Hitting.Kl,
                Pickoff = x.Hitting.Pickoff,
                R = x.Hitting.R,
                Rbi = x.Hitting.Rbi,
                Rchci = x.Hitting.Rchci,
                Rcherr = x.Hitting.Rcherr,
                Sb = x.Hitting.Sb,
                Sf = x.Hitting.Sf,
                Sh = x.Hitting.Sh,
                So = x.Hitting.So,
                Triple = x.Hitting.Triple,
            },
            Fielding = null,
            Found = false,
        }).ToList();
    }
}


public class GameChangerXmlPeeker
{
    public GameChangerFileOverview GetFileOverviewFromXml(string xml)
    {
        // Create an XmlDocument instance and load the XML string
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        // Get the root element (Person) of the XML document
        XmlNode? gameNode = xmlDoc.SelectSingleNode("bsgame");

        if (gameNode is null)
            throw new Exception("Could not parse gamechanger file. Could not find `bsgame` node.");

        var venue = gameNode.SelectSingleNode("venue");

        if (venue is null)
            throw new Exception("Could not parse gamechanger file. Could not find `venue` node.");

        var overview = new GameChangerFileOverview
        {
            GameId = venue.Attributes!.GetNamedItem("gameid")!.Value,
            HomeTeam = venue.Attributes!.GetNamedItem("homename")!.Value,
            AwayTeam = venue.Attributes!.GetNamedItem("visname")!.Value,
            GameDate = ParseDate(venue.Attributes!.GetNamedItem("date")!.Value!),
            Format = ParseFormat(gameNode.Attributes!.GetNamedItem("source_format")!.Value!)
        };

        return overview;
    }


    internal GameChangerFormat ParseFormat(string formatString) => formatString switch
    {
        "Chelsea" => GameChangerFormat.Chelsea,
        "Sabertooth" => GameChangerFormat.Sabertooth,
        _ => throw new Exception("")
    };

    internal DateTime ParseDate(string dateString)
    {
        const string DATE_FORMAT = "MM/dd/yy";
        DateTime result;

        if (!DateTime.TryParseExact(dateString, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            throw new Exception("Failed to parse game date");
        }

        return result;
    }


}

public class GameChangerFileOverview
{
    public string? HomeTeam { get; set; }
    public string? AwayTeam { get; set; }
    public string? GameId { get; set; }
    public DateTime GameDate { get; set; }
    public GameChangerFormat Format { get; set; }
}

public enum GameChangerFormat
{
    Sabertooth = 1,
    Chelsea = 2,
}