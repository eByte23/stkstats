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





public class InMemoryGameUploadRepository : IGameUploadRepository {
    private List<GameUpload> _gameUploads;

    public InMemoryGameUploadRepository(List<GameUpload>? gameUploads = null) {
        _gameUploads = gameUploads ?? new List<GameUpload>();
    }

    public async Task<bool> AddGameUploadAsync(GameUpload gameUpload) {
        _gameUploads.Add(gameUpload);
        return true;
    }

    public async Task<bool> DeleteGameUploadAsync(Guid id) {
        var gameUpload = _gameUploads.FirstOrDefault(x => x.Id == id);
        if (gameUpload == null) {
            return false;
        }
        _gameUploads.Remove(gameUpload);
        return true;
    }

    public async Task<List<GameUpload>> GetGameUploadsAsync() {
        return _gameUploads;
    }

    public async Task<GameUpload?> GetGameUploadAsync(Guid id) {
        return _gameUploads.FirstOrDefault(x => x.Id == id);
    }

    public async Task<bool> UpdateGameUploadAsync(GameUpload gameUpload) {
        var existingGameUpload = _gameUploads.FirstOrDefault(x => x.Id == gameUpload.Id);
        if (existingGameUpload == null) {
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