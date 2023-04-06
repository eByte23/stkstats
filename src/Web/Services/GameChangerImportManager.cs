namespace STKBC.Stats.Services;


public class GameChangerImportManager
{
    private UniqueIdGenerator _uniqueIdGenerator;
    private IClock _clock;
    private IFileService _fileService;

    public GameChangerImportManager(UniqueIdGenerator uniqueIdGenerator, IClock clock, IFileService fileService)
    {
        this._uniqueIdGenerator = uniqueIdGenerator;
        this._clock = clock;
        this._fileService = fileService;
    }

    public object CreateImportRequestFromFileId(Guid? id)
    {
        throw new NotImplementedException();
    }
}