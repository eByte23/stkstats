using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using STKBC.Stats.Data.Models;
using STKBC.Stats.Services;
using STKBC.Stats.Services.FileStorage;
using STKBC.Stats.Repositories;

namespace STKBC.Tests.Services;

public class FileUploadServiceTests
{
    [Fact]
    public async Task UploadFileAsync_ShouldStoreFileInStorage_AndCreateFileUploadModel()
    {

        const string fileData = "I AM A TEST HASH FILE!";
        const string fileHash = "b4fa41bfc64954db3f4ab9f2d758539c260dcf5ffc773427371658ca6abee049";

        var fileStoreMock = new Mock<IFileStore>();
        var fileUploadRepositoryMock = new Mock<IFileUploadRepository>();

        var fileUploadService = new FileUploadService(fileStoreMock.Object, fileUploadRepositoryMock.Object);
        var fileId = new UniqueIdGenerator().NewDeterministicId("file-1");

        var fileUploadStream = new MemoryStream(Encoding.UTF8.GetBytes(fileData));

        await fileUploadService.UploadFileAsync(
                        new FileUploadModel
                        {
                            FileId = fileId.Id,
                            FileName = "file-1.txt",
                            FileSize = 100,
                            FileContentType = "text/plain"
                        },
                        fileUploadStream
                    );

        fileStoreMock
            .Verify(x => x.SaveFileAsync(
                It.Is<string>(s => s == fileId.String()),
                It.IsAny<Stream>()),
                Times.Once
            );

        FileUpload expectedFileUpload = new FileUpload
        {
            Id = fileId.Id,
            Extension = ".txt",
            Hash = fileHash,
            Location = fileId.Id.ToString(),
            Name = "file-1.txt",
            Size = 100
        };

        fileUploadRepositoryMock
            .Verify(x => x.CreateAsync(
                It.Is<FileUpload>(m => 
                    m.Id == expectedFileUpload.Id &&
                    m.Extension == expectedFileUpload.Extension &&
                    m.Hash == expectedFileUpload.Hash &&
                    m.Location == expectedFileUpload.Location &&
                    m.Name == expectedFileUpload.Name &&
                    m.Size == expectedFileUpload.Size
                )),
                Times.Once);
    }
}