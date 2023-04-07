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
    public FileObject? GetFileObject(Guid? fileId);

    public IFileObjectStream GetFileObjectStream(Guid? fileId);

    public IFileObjectStream GetFileObjectStream(FileObject? fileObject);
}


public class FileService : IFileService
{
    public FileObject? GetFileObject(Guid? fileId)
    {
        throw new NotImplementedException();
    }

    public IFileObjectStream GetFileObjectStream(Guid? fileId)
    {
        throw new NotImplementedException();
    }

    public IFileObjectStream GetFileObjectStream(FileObject? fileObject)
    {
        throw new NotImplementedException();
    }
}