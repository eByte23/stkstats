using STKBC.Stats.Data.Models;
using STKBC.Stats.Services;


namespace STKBC.Tests.Imports.GameChanger;


public class SabertoothFormatImportTests
{


    [Fact]
    public void GameChangerImportUtil_Should_CreateAValidImportRquest()
    {
        var filePath = "old-games/c res/St.Kilda 2019 MWBL C Resv Grade vs Doncaster 05_04_19 Stats.xml";

        var fakeIdGenerator = new Mock<IIdGenerator>();
        var fakeFrozenClock = new Mock<IClock>();
        var fakeFileService = new Mock<IFileService>();
        var fakeFileObjectStream = new Mock<IFileObjectStream>();

        fakeIdGenerator
            .Setup(x => x.NewGuid())
            .Returns(new Guid("771e2e3c-3eae-42ef-a8c7-b407724846d1"));

        fakeFrozenClock
            .Setup(x => x.GetUtcNow())
            .Returns(new DateTime(2023, 4, 7, 9, 0, 0));

        fakeFileObjectStream
            .Setup(x => x.GetText())
            .Returns(LocalFileHelpers.GetFileText(filePath));

        Guid fileId = new Guid("0f33944b-5598-4a7c-aa5e-e602b38f29a7");

        fakeFileService
            .Setup(x => x.GetFileObject(It.Is<Guid>(x => x == fileId)))
            .Returns(new FileObject
            {
                Id = fileId,
                Name = "St.Kilda 2019 MWBL C Resv Grade vs Doncaster 05_04_19 Stats.xml",
                Extension = "xml",
                Size = 123456,
                Hash = "file-hash",
                Location = "/bucket/random/location/filename",
            });

        fakeFileService
            .Setup(x => x.GetFileObjectStream(It.Is<Guid>(x => x == fileId)))
            .Returns(fakeFileObjectStream.Object);

        var gameChangerImportManager = new GameChangerImportManager(
            fakeIdGenerator.Object,
            fakeFrozenClock.Object,
            fakeFileService.Object
        );

        var importRequest = gameChangerImportManager.CreateImportRequestFromFileId(new Guid("0f33944b-5598-4a7c-aa5e-e602b38f29a7"));

        Assert.Equivalent(new FileImportRequest
        {
            Id = new Guid("771e2e3c-3eae-42ef-a8c7-b407724846d1"),
            GameDate = new DateTime(2019, 5, 4, 0, 0, 0),
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



    [Fact]
    public void GameChangerImportManager_Should_ParseToTemporaryGameUploadModel()
    {
        var namespaceGuid = new Guid("771e2e3c-3eae-42ef-a8c7-b407724846d1");

        var filePath = "test-games/test1_sabertooth_shortened_players.xml";
        var fakeIdGenerator = new Mock<IIdGenerator>();
        var fakeFrozenClock = new Mock<IClock>();
        var fakeFileService = new Mock<IFileService>();
        var fakeFileObjectStream = new Mock<IFileObjectStream>();

        var idGen = new UniqueIdGenerator();

        fakeIdGenerator
            .Setup(x => x.NewGuid())
            .Returns(idGen.NewDeterministicGuid(namespaceGuid, "temp-game-upload").Id);

        fakeIdGenerator
            .Setup(x => x.NewDeterministicGuid(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns((Guid id, string val) =>
            {
                return idGen.NewDeterministicGuid(id, val);
            });

        fakeFrozenClock
            .Setup(x => x.GetUtcNow())
            .Returns(new DateTime(2023, 4, 7, 15, 0, 0));

        fakeFileObjectStream
            .Setup(x => x.GetText())
            .Returns(LocalFileHelpers.GetFileText(filePath));

        Guid fileId = new Guid("0410ba21-8f2d-4e8a-9af5-2b06dfc670e3");

        fakeFileService
            .Setup(x => x.GetFileObjectStream(It.Is<Guid>(x => x == fileId)))
                .Returns(fakeFileObjectStream.Object);



        var gameChangerImportManager = new GameChangerImportManager(
           fakeIdGenerator.Object,
           fakeFrozenClock.Object,
           fakeFileService.Object
       );

        var importRequest = new FileImportRequest
        {
            Id = new Guid("771e2e3c-3eae-42ef-a8c7-b407724846d1"),
            GameDate = new DateTime(2019, 5, 4, 0, 0, 0),
            ExternalRef = "5cccbb850cd201f5ec000008",
            FileName = "St.Kilda 2019 MWBL C Resv Grade vs Doncaster 05_04_19 Stats.xml",
            HomeTeam = "home-team",
            AwayTeam = "away-team",
            ImportType = "GameChanger_Sabertooth",
            FileId = new Guid("0410ba21-8f2d-4e8a-9af5-2b06dfc670e3"),
            FileHash = "file-hash",
            UploadedAt = new DateTime(2023, 4, 7, 9, 0, 0)
        };

        var temp = gameChangerImportManager.GetTemporaryGameUploadFromImportRequest(importRequest);

        var tempGameId = idGen.NewDeterministicGuid(namespaceGuid, "temp-game-upload");
        var homeTeamGuid = tempGameId.NewGuid("home-team");
        var awayTeamGuid = tempGameId.NewGuid("away-team");

        Assert.Equivalent(
                new TemporaryGameUpload
                {
                    Id = tempGameId.Id,
                    ImportRequestId = new Guid("771e2e3c-3eae-42ef-a8c7-b407724846d1"),
                    CreatedAt = new DateTime(2023, 4, 7, 15, 0, 0),
                    GameDate = new DateTime(2019, 5, 4, 0, 0, 0),
                    FileId = new Guid("0410ba21-8f2d-4e8a-9af5-2b06dfc670e3"),
                    FileName = "St.Kilda 2019 MWBL C Resv Grade vs Doncaster 05_04_19 Stats.xml",
                    HomeTeam = new()
                    {
                        Id = homeTeamGuid.Id,
                        Found = false,
                        Name = "home-team",
                        Players = new List<TemporaryPlayer>(){
                        new TemporaryPlayer()
                        {
                            PlayerId = homeTeamGuid.NewGuid("SNTS-Player 1").Id,
                            Found = false,
                            DisplayName = "SNTS-Player 1",
                            Batting = null,
                            Fielding = null,
                            Pitching = null,
                        },
                        new TemporaryPlayer()
                        {
                            PlayerId = homeTeamGuid.NewGuid("SNTS-Player 2").Id,
                            Found = false,
                            DisplayName = "SNTS-Player 2",
                            Batting = null,
                            Fielding = null,
                            Pitching = null,
                        },
                        new TemporaryPlayer()
                        {
                            PlayerId = homeTeamGuid.NewGuid("SNTS-Player 3").Id,
                            Found = false,
                            DisplayName = "SNTS-Player 3",
                            Batting = null,
                            Fielding = null,
                            Pitching = null,
                        },
                        }
                    },
                    AwayTeam = new()
                    {
                        Id = awayTeamGuid.Id,
                        Found = false,
                        Name = "away-team",
                        Players = new List<TemporaryPlayer>(){
                        new TemporaryPlayer(){
                            PlayerId = awayTeamGuid.NewGuid("DNCS-Player 1").Id,
                            Found = false,
                            DisplayName = "DNCS-Player 1",
                            Batting = null,
                            Fielding = null,
                            Pitching = null,
                        },
                        new TemporaryPlayer(){
                            PlayerId = awayTeamGuid.NewGuid("DNCS-Player 2").Id,
                            Found = false,
                            DisplayName = "DNCS-Player 2",
                            Batting = null,
                            Fielding = null,
                            Pitching = null,
                        },
                        new TemporaryPlayer(){
                           PlayerId = awayTeamGuid.NewGuid("DNCS-Player 3").Id,
                            Found = false,
                            DisplayName = "DNCS-Player 3",
                            Batting = null,
                            Fielding = null,
                            Pitching = null,
                        },
                        }
                    },




                },
                temp
            );
    }

}