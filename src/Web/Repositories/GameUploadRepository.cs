using STKBC.Stats.Data.Models;

namespace STKBC.Stats.Repositories;

public interface IGameUploadRepository
{
    Task<bool> AddGameUploadAsync(GameUpload gameUpload);
    Task<bool> DeleteGameUploadAsync(Guid id);
    Task<List<GameUpload>> GetGameUploadsAsync();
    Task<GameUpload?> GetGameUploadAsync(Guid id);
    Task<bool> UpdateGameUploadAsync(GameUpload gameUpload);
}





public class InMemoryGameUploadRepository : IGameUploadRepository
{
    private List<GameUpload> _gameUploads;

    public InMemoryGameUploadRepository(List<GameUpload>? gameUploads = null)
    {
        _gameUploads = gameUploads ?? new List<GameUpload>();
    }

    public async Task<bool> AddGameUploadAsync(GameUpload gameUpload)
    {
        _gameUploads.Add(gameUpload);
        return true;
    }

    public async Task<bool> DeleteGameUploadAsync(Guid id)
    {
        var gameUpload = _gameUploads.FirstOrDefault(x => x.Id == id);
        if (gameUpload == null)
        {
            return false;
        }
        _gameUploads.Remove(gameUpload);
        return true;
    }

    public async Task<List<GameUpload>> GetGameUploadsAsync()
    {
        return _gameUploads;
    }

    public async Task<GameUpload?> GetGameUploadAsync(Guid id)
    {
        return _gameUploads.FirstOrDefault(x => x.Id == id);
    }

    public async Task<bool> UpdateGameUploadAsync(GameUpload gameUpload)
    {
        var existingGameUpload = _gameUploads.FirstOrDefault(x => x.Id == gameUpload.Id);
        if (existingGameUpload == null)
        {
            throw new Exception("GameUpload not found");
        }
        existingGameUpload.GameDate = gameUpload.GameDate;
        existingGameUpload.ExternalRef = gameUpload.ExternalRef;
        existingGameUpload.FileName = gameUpload.FileName;
        existingGameUpload.HomeTeam = gameUpload.HomeTeam;
        existingGameUpload.AwayTeam = gameUpload.AwayTeam;
        existingGameUpload.ImportType = gameUpload.ImportType;
        existingGameUpload.FileId = gameUpload.FileId;
        existingGameUpload.FileHash = gameUpload.FileHash;
        existingGameUpload.UploadedAt = gameUpload.UploadedAt;
        return true;
    }
}



public class LocalStorageFileGameUploadRepository : IGameUploadRepository
{
    private readonly RepoFileSystemStorage<GameUpload> _repoFileSystemStorageHelper;

    public LocalStorageFileGameUploadRepository(RepoFileSystemStorageHelper storageHelper)
    {
        _repoFileSystemStorageHelper = storageHelper.GetRepoFileSystemStorage<GameUpload>();
    }

    public async Task<bool> AddGameUploadAsync(GameUpload gameUpload)
    {

        var gameUploads = await _repoFileSystemStorageHelper.GetAllAsync();

        gameUploads.Add(gameUpload);

        await _repoFileSystemStorageHelper.SaveAllAsync(gameUploads);

        return true;
    }

    public Task<bool> DeleteGameUploadAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<GameUpload?> GetGameUploadAsync(Guid id)
    {
        var gameUploads = await _repoFileSystemStorageHelper.GetAllAsync();

        return gameUploads.SingleOrDefault(x => x.Id == id);
    }

    public async Task<List<GameUpload>> GetGameUploadsAsync()
    {
        var gameUploads = await _repoFileSystemStorageHelper.GetAllAsync();

        return gameUploads;
    }

    public async Task<bool> UpdateGameUploadAsync(GameUpload gameUpload)
    {

        var gameUploads = await _repoFileSystemStorageHelper.GetAllAsync();
        var existingGameUpload = gameUploads.FirstOrDefault(x => x.Id == gameUpload.Id);
        if (existingGameUpload == null)
        {
            throw new Exception("GameUpload not found");
        }
        existingGameUpload.GamePreviewId = gameUpload.GamePreviewId;
        existingGameUpload.GameDate = gameUpload.GameDate;
        existingGameUpload.ExternalRef = gameUpload.ExternalRef;
        existingGameUpload.FileName = gameUpload.FileName;
        existingGameUpload.HomeTeam = gameUpload.HomeTeam;
        existingGameUpload.AwayTeam = gameUpload.AwayTeam;
        existingGameUpload.ImportType = gameUpload.ImportType;
        existingGameUpload.FileId = gameUpload.FileId;
        existingGameUpload.FileHash = gameUpload.FileHash;
        existingGameUpload.UploadedAt = gameUpload.UploadedAt;

        await _repoFileSystemStorageHelper.SaveAllAsync(gameUploads);
        return true;
    }
}
