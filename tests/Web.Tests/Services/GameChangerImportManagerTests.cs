using STKBC.Stats.Data.Models;
using STKBC.Stats.Services;

namespace STKBC.Tests.Services;

public class GameChangerImportManagerTests
{


    [Fact]
    public void GameChangerImportUtil_Should_CreateAValidImportRquest()
    {
        var filePath = "/old-games/c res/St.Kilda 2019 MWBL C Resv Grade vs Doncaster 05_04_19 Stats.xml";

        var fileData = DateFileHelpers.GetFileText(filePath);
        var fileName = DateFileHelpers.GetFileName(filePath);
        var game = Stats.Parser.ParserUtil.Deserialize(fileData);


        var fakeIdGenerator = new Mock<UniqueIdGenerator>();
        var fakeFrozenClock = new Mock<IClock>();
        var fakeFileService = new Mock<IFileService>();
        var fakeFileObjectStream = new Mock<FileObjectStream>();

        var fileObject = new FileObject
        {
            Id = new Guid("0f33944b-5598-4a7c-aa5e-e602b38f29a7"),
            Name = "St.Kilda 2019 MWBL C Resv Grade vs Doncaster 05_04_19 Stats.xml",
            Extension = "xml",
            Size = 123456,
            Hash = "file-hash",
            Location = "/bucket/random/location/filename",
        };

        fakeIdGenerator
            .Setup(x => x.NewGuid())
            .Returns(new Guid("771e2e3c-3eae-42ef-a8c7-b407724846d1"));

        fakeFrozenClock
            .Setup(x => x.GetUtcNow())
            .Returns(new DateTime(2023, 4, 7, 9, 0, 0));



        fakeFileObjectStream
            .Setup(x => x.GetText())
            .Returns(File.ReadAllText(filePath));

        fakeFileService
            .Setup(x => x.GetFileObject(It.Is<Guid>(x => x == fileObject.Id)))
            .Returns(fileObject);

        fakeFileService
            .Setup(x => x.GetFileObjectStream(It.Is<Guid>(x => x == fileObject.Id)))
            .Returns(fakeFileObjectStream.Object);


        // var gameChangerImportManager = new GameChangerImportManager(new FakeIdGenerator(), new FrozenClock(), fakeFileService);
        var gameChangerImportManager = new GameChangerImportManager(
            fakeIdGenerator.Object,
            fakeFrozenClock.Object,
            fakeFileService.Object
        );

        var importRequest = gameChangerImportManager.CreateImportRequestFromFileId(fileObject.Id);

        Assert.Equivalent(new FileImportRequest
        {
            Id = new Guid("771e2e3c-3eae-42ef-a8c7-b407724846d1"),
            GameDate = new DateTime(2019, 4, 5, 0, 0, 0),
            ExternalRef = "5cccbb850cd201f5ec000008",
            FileName = "St.Kilda 2019 MWBL C Resv Grade vs Doncaster 05_04_19 Stats.xml",
            HomeTeam = "St.Kilda 2019 MWBL C Resv Grade",
            AwayTeam = "Doncaster",
            ImportType = "GameChanger_Sabertooth",
            FileId = new Guid("0f33944b-5598-4a7c-aa5e-e602b38f29a7"),
            FileHash = "file-hash",
            UploadedAt = new DateTime(2023, 4, 7, 9, 0, 0)
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
