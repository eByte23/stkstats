namespace STKBC.Stats.Web.Tests.Pages.GameUploadRequests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Logging;
    using Moq;
    using STKBC.Stats.Data;
    using STKBC.Stats.Data.Models;
    using STKBC.Stats.Pages.GameUploadRequests;
    using STKBC.Stats.Services;
    using STKBC.Stats.Services.FileStorage;
    using STKBC.Stats.Repositories;
    // using STKBC.Stats.Web.Pages.GameUploadRequests;
    using Xunit;

    public class UploadPageTests
    {
        //         [Fact]
        //         public async Task OnPostAsync_WhenFileIsNull_ReturnsPageResult()
        //         {
        //             // Arrange
        //             var logger = new Mock<ILogger<UploadModel>>();
        //             var fileStore = new Mock<IFileStore>();
        //             var statsDb = new Mock<StatsDb>();
        //             var pageModel = new UploadModel(logger.Object, fileStore.Object, statsDb.Object);

        //             // Act
        //             var result = await pageModel.OnPostAsync(null);

        //             // Assert
        //             Assert.IsType<PageResult>(result);
        //         }

        //         [Fact]
        //         public async Task OnPostAsync_WhenFileIsNotNull_ReturnsRedirectToPageResult()
        //         {
        //             // Arrange
        //             var logger = new Mock<ILogger<UploadModel>>();
        //             var fileStore = new Mock<IFileStore>();
        //             var statsDb = new Mock<StatsDb>();
        //             var pageModel = new UploadModel(logger.Object, fileStore.Object, statsDb.Object);

        //             // Act
        //             var result = await pageModel.OnPostAsync(new Microsoft.AspNetCore.Http.FormFile(null, 0, 0, null, null));

        //             // Assert
        //             Assert.IsType<RedirectToPageResult>(result);
        //         }

        //         [Fact]
        //         public async Task OnPostAsync_WhenFileIsNotNull_SavesFile()
        //         {
        //             // Arrange
        //             var logger = new Mock<ILogger<UploadModel>>();
        //             var fileStore = new Mock<IFileStore>();
        //             var statsDb = new Mock<StatsDb>();
        //             var pageModel = new UploadModel(logger.Object, fileStore.Object, statsDb.Object);

        //             // Act
        //             var result = await pageModel.OnPostAsync(new Microsoft.AspNetCore.Http.FormFile(null, 0, 0, null, null));

        //             // Assert
        //             fileStore.Verify(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
        //         }

        [Fact]
        public async Task OnPostAsync_WhenFileIsNotNull_SavesFileWithCorrectName()
        {
            const string fileData = "I AM A TEST HASH FILE!";
            const string fileHash = "b4fa41bfc64954db3f4ab9f2d758539c260dcf5ffc773427371658ca6abee049";

            // Arrange
            var logger = new Mock<ILogger<UploadModel>>();
            var fileStore = new InMemoryFileStore();
            var fileUploadRepository = new InMemoryFileUploadRepository();
            var fileUploadService = new FileUploadService(fileStore, fileUploadRepository);
            var statsDb = new Mock<StatsDb>();
            var pageModel = new UploadModel(fileUploadService);

            // Act

            pageModel.FileId = Guid.NewGuid();
            Microsoft.AspNetCore.Http.FormFile formFile = new Microsoft.AspNetCore.Http.FormFile(
                            new MemoryStream(Encoding.UTF8.GetBytes(fileData)),
                            0,
                            fileData.Length,
                            "file-1.txt",
                            "file-1.txt"
                        ){
                            Headers = new HeaderDictionary(),
                            ContentType = "text/plain"
                        };


            pageModel.GameFile = formFile;


            pageModel.FileType = GameType.GameChanger;


            var result = await pageModel.OnPostAsync();

            // // Assert
            var fileUpload = await fileUploadRepository.GetAsync(pageModel.FileId.Value);
            Assert.NotNull(fileUpload);


            Assert.Equal(fileUpload.Id, pageModel.FileId);
            Assert.Equal(fileUpload.Hash, fileHash);
            Assert.Equal(fileUpload.Name, "file-1.txt");
            Assert.Equal(fileUpload.Extension, ".txt");
            // Assert.Equal(fileUpload.ContentType, "text/plain");


            var file = await fileStore.GetFileStreamAsync(fileUpload.Id!.Value.ToString());
            Assert.NotNull(file);
            Assert.Equal(file.Length, fileData.Length);

            var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileData2 = Encoding.UTF8.GetString(ms.ToArray());
            Assert.Equal(fileData, fileData2);
        }
    }
}