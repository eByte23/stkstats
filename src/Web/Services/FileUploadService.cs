using System.Security.Cryptography;
using STKBC.Stats.Services.FileStorage;
using STKBC.Stats.Repositories;

namespace STKBC.Stats.Services;

public interface IFileUploadService
{
    Task UploadFileAsync(FileUploadModel fileUploadModel, Stream fileUploadStream);
}


public class FileUploadService : IFileUploadService
{
    private readonly IFileStore _fileStore;
    private readonly IFileUploadRepository _fileUploadRepository;

    public FileUploadService(IFileStore fileStore, IFileUploadRepository fileUploadRepository)
    {
        this._fileStore = fileStore;
        this._fileUploadRepository = fileUploadRepository;
    }

    public async Task UploadFileAsync(FileUploadModel fileUploadModel, Stream fileUploadStream)
    {
        using var sha256 = SHA256.Create();
        await using (var memoryStream = new MemoryStream())
        await using (var cryptoStream = new CryptoStream(memoryStream, sha256, CryptoStreamMode.Write))
        {
            await fileUploadStream.CopyToAsync(cryptoStream);
            memoryStream.Position = 0;
            await this._fileStore.SaveFileAsync(fileUploadModel.FileId!.Value.ToString(), memoryStream);
        }

        var fileHash = BitConverter.ToString(sha256.Hash!).Replace("-", "").ToLowerInvariant();

        await this._fileUploadRepository.CreateAsync(new Stats.Data.Models.FileUpload
        {
            Id = fileUploadModel.FileId,
            Name = fileUploadModel.FileName,
            Hash = fileHash,
            Size = fileUploadModel.FileSize,
            // FileMimeType = fileUploadModel.FileMimeType,
            Location = fileUploadModel.FileId!.Value.ToString(),
            Extension = Path.GetExtension(fileUploadModel.FileName),
            // FileUploadDate = DateTime.UtcNow
        });
    }
}

public class FileUploadModel
{
    public Guid? FileId { get; set; }
    public string? FileName { get; set; }
    public int FileSize { get; set; }
    public string? FileContentType { get; set; }
}
