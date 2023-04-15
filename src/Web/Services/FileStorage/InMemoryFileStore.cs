namespace STKBC.Stats.Services.FileStorage;

public class InMemoryFileStore : IFileStore
{
    private readonly Dictionary<string, byte[]> _files = new Dictionary<string, byte[]>();

    public Task<bool> DeleteFileAsync(string fileName)
    {
        _files.Remove(fileName);
        return Task.FromResult(true);
    }

    public Task<bool> FileExistsAsync(string fileName)
    {
        return Task.FromResult(_files.ContainsKey(fileName));
    }

    public Task<Stream?> GetFileStreamAsync(string fileName)
    {
        if (_files.TryGetValue(fileName, out var file))
        {
            return Task.FromResult<Stream?>(new MemoryStream(file));
        }

        return Task.FromResult<Stream?>(null);
    }

    public Task<string> GetFileUrlAsync(string fileName)
    {
        return Task.FromResult(fileName);
    }

    public Task<bool> SaveFileAsync(string fileName, Stream fileStream)
    {
        using (var memoryStream = new MemoryStream())
        {
            fileStream.CopyTo(memoryStream);
            _files[fileName] = memoryStream.ToArray();
        }

        return Task.FromResult(true);
    }
}