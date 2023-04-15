namespace STKBC.Stats.Services.FileStorage;

public class FileSystemFileStore : IFileStore
{
    private readonly string _rootPath;

    public FileSystemFileStore(string rootPath)
    {
        _rootPath = rootPath;
    }

    public Task<bool> DeleteFileAsync(string fileName)
    {
        if (!File.Exists(Path.Combine(_rootPath, fileName)))
        {
            return Task.FromResult(false);
        }

        File.Delete(Path.Combine(_rootPath, fileName));
        
        return Task.FromResult(true);
    }

    public Task<bool> FileExistsAsync(string fileName)
    {
        return Task.FromResult(File.Exists(Path.Combine(_rootPath, fileName)));
    }

    public Task<Stream?> GetFileStreamAsync(string fileName)
    {
        if (!File.Exists(Path.Combine(_rootPath, fileName)))
        {
            return Task.FromResult<Stream?>(null);
        }

        return Task.FromResult<Stream?>(File.OpenRead(Path.Combine(_rootPath, fileName)));
    }

    public Task<string> GetFileUrlAsync(string fileName)
    {
        return Task.FromResult(Path.Combine(_rootPath, fileName));
    }

    public Task<bool> SaveFileAsync(string fileName, Stream fileStream)
    {
        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }

        var filePath = Path.Combine(_rootPath, fileName);

        using (var file = File.Create(filePath))
        {
            fileStream.CopyTo(file);
        }

        return Task.FromResult(true);
    }
}