namespace STKBC.Tests.Imports.GameChanger;


public class SabertoothFormatImportTests
{


    [Fact]
    public void GameChangerImportUtil_Should_CreateAValidImportRquest()
    {
        var filePath = "/old-games/c res/St.Kilda 2019 MWBL C Resv Grade vs Doncaster 05_04_19 Stats.xml";

        var fileData = DateFileHelpers.GetFileText(filePath);
        var fileName = DateFileHelpers.GetFileName(filePath);
        var game = Stats.Parser.ParserUtil.Deserialize(fileData);


        var importRequest = GameChangerImportManager.CreateImportRequestFromFile(fileName, fileData);

        Assert.Equivalent(new FileImportRequest
        {
            Id = new Guid(""),
            GameDate = new DateTime(2019, 04, 05),
            ExternalRef = "5cccbb850cd201f5ec000008",
            FileName = "St.Kilda 2019 MWBL C Resv Grade vs Doncaster 05_04_19 Stats.xml",
            HomeTeam = "",
            AwayTeam = "",
            ImportType = "GameChanger_Sabertooth",
            FileId = new Guid(""),
            FileHash = "",
            UploadedAt = new DateTime()
        }, importRequest);
    }

}


internal static class DateFileHelpers
{
    const string DATA_PATH = "/workspace/stats/data/";

    internal static string GetFileText(string filePath)
    {
        var absolutePath = Path.Join(DATA_PATH, filePath);

        return File.ReadAllText(absolutePath);
    }

    internal static string GetFileName(string filePath)
    {
        return Path.GetFileName(filePath);
    }
}
