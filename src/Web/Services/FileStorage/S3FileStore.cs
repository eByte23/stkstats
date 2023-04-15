namespace STKBC.Stats.Services.FileStorage;

public interface IFileStore
{
    Task<bool> DeleteFileAsync(string path);
    Task<bool> FileExistsAsync(string path);
    Task<string> GetFileUrlAsync(string path);
    Task<Stream?> GetFileStreamAsync(string path);
    Task<bool> SaveFileAsync(string path, Stream fileStream);
}

public class S3FileStore : IFileStore
{
    public Task<bool> DeleteFileAsync(string path)
    {
        throw new NotImplementedException();
    }

    public Task<bool> FileExistsAsync(string path)
    {
        throw new NotImplementedException();
    }

    public Task<Stream?> GetFileStreamAsync(string path)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetFileUrlAsync(string path)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveFileAsync(string path, Stream fileStream)
    {
        throw new NotImplementedException();
    }
}