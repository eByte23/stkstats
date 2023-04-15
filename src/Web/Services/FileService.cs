using STKBC.Stats.Repositories;
using STKBC.Stats.Services.FileStorage;

namespace STKBC.Stats.Services;

public class FileObject
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Extension { get; set; }
    public int? Size { get; set; }
    public string? Hash { get; set; }
    public string? Location { get; set; }
}

public interface IFileObjectStream
{
    string GetText();
    Stream Stream { get; }
}


public class FileObjectStream : IFileObjectStream, IDisposable
{
    private readonly Stream _fileStream;
    private readonly StreamReader _reader;

    public FileObjectStream(Stream fileStream)
    {
        _fileStream = fileStream;
        _reader = new StreamReader(_fileStream);
    }

    public Stream Stream => _fileStream;

    public void Dispose()
    {
        if (_reader != null)
        {
            _reader.Dispose();
        }

        if (_fileStream != null)
        {
            _fileStream.Dispose();
        }
    }

    public string GetText()
    {
        _fileStream.Seek(0, SeekOrigin.Begin);
        string text = _reader.ReadToEnd();

        return text;
    }
}


public interface IFileService
{
    public Task<FileObject?> GetFileObjectAsync(Guid? fileId);

    public Task<IFileObjectStream> GetFileObjectStream(Guid? fileId);

    public Task<IFileObjectStream> GetFileObjectStream(FileObject? fileObject);
}


public class FileService : IFileService
{
    private readonly IFileStore _store;
    private readonly IFileUploadRepository _fileUploadRepository;

    public FileService(IFileStore fileStore, IFileUploadRepository fileUploadRepository)
    {
        this._store = fileStore;
        this._fileUploadRepository = fileUploadRepository;
    }

    public async Task<FileObject?> GetFileObjectAsync(Guid? fileId)
    {
        if (fileId == null)
        {
            return null;
        }

        var file = await _fileUploadRepository.GetAsync(fileId.Value);

        if (file == null)
        {
            return null;
        }

        return new FileObject
        {
            Id = file.Id,
            Name = file.Name,
            Extension = file.Extension,
            Size = file.Size,
            Hash = file.Hash,
            Location = file.Location
        };
    }

    public async Task<IFileObjectStream> GetFileObjectStream(Guid? fileId)
    {

        if (fileId == null)
        {
            throw new ArgumentNullException(nameof(fileId));
        }

        var file = await _fileUploadRepository.GetAsync(fileId.Value);

        if (file == null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        Stream? fileStream = await _store.GetFileStreamAsync(file.Location!);
        
        return new FileObjectStream(fileStream!);
    }

    public Task<IFileObjectStream> GetFileObjectStream(FileObject? fileObject)
    {
        throw new NotImplementedException();
    }
}