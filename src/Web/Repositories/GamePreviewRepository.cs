using STKBC.Stats.Data.Models;

namespace STKBC.Stats.Repositories;

public interface IGamePreviewRepository
{
    Task<GamePreview?> GetGamePreviewAsync(Guid gameId);
    Task<bool> CreateGamePreviewAsync(GamePreview gamePreview);
    Task<bool> UpdateGamePreviewAsync(GamePreview gamePreview);

}

public class InMemoryGamePreviewRepository : IGamePreviewRepository
{
    private List<GamePreview> _games = new List<GamePreview>
    {
    };


    public InMemoryGamePreviewRepository()
    {

    }

    public async Task<GamePreview?> GetGamePreviewAsync(Guid gameId)
    {
        return _games.SingleOrDefault(x => x.GameId == gameId);
        // var game = await _gameRepository.GetAsync(gameId);
        // if (game == null)
        // {
        //     return null;
        // }

        // var homeTeam = await _teamRepository.GetAsync(game.HomeTeamId);
        // var awayTeam = await _teamRepository.GetAsync(game.AwayTeamId);

        // var homeTeamPlayers = await _playerRepository.GetPlayersByTeamAsync(game.HomeTeamId);
        // var awayTeamPlayers = await _playerRepository.GetPlayersByTeamAsync(game.AwayTeamId);

        // var homeTeamPreview = new GamePreview
        // {
        //     TeamId = homeTeam.Id,
        //     TeamName = homeTeam.Name,
        //     Players = homeTeamPlayers.Select(x => new GamePreview
        //     {
        //         PlayerId = x.Id,
        //         PlayerName = x.Name,
        //         PlayerNumber = x.Number
        //     }).ToList()
        // };

        // var awayTeamPreview = new GamePreview
        // {
        //     TeamId = awayTeam.Id,
        //     TeamName = awayTeam.Name,
        //     Players = awayTeamPlayers.Select(x => new PlayerPreviewView
        //     {
        //         PlayerId = x.Id,
        //         PlayerName = x.Name,
        //         PlayerNumber = x.Number
        //     }).ToList()
        // };

        // return new GamePreview
        // {
        //     GameId = game.Id,
        //     GradeId = game.GradeId,
        //     HomeTeam = homeTeamPreview,
        //     AwayTeam = awayTeamPreview
        // };
    }

    public async Task<bool> CreateGamePreviewAsync(GamePreview gamePreview)
    {
        _games.Add(gamePreview);
        return true;
    }

    public async Task<bool> UpdateGamePreviewAsync(GamePreview gamePreview)
    {
        var game = _games.SingleOrDefault(x => x.GameId == gamePreview.GameId);
        if (game == null)
        {
            return false;
        }
        game.GradeId = gamePreview.GradeId;
        game.HomeTeam = gamePreview.HomeTeam;
        game.AwayTeam = gamePreview.AwayTeam;
        return true;
    }
}






public class LocalStorageFileGamePreviewRepository : IGamePreviewRepository
{
    private readonly RepoFileSystemStorage<GamePreview> _repoFileSystemStorageHelper;

    public LocalStorageFileGamePreviewRepository(RepoFileSystemStorageHelper storageHelper)
    {
        _repoFileSystemStorageHelper = storageHelper.GetRepoFileSystemStorage<GamePreview>();
    }

    public async Task<bool> CreateGamePreviewAsync(GamePreview gamePreview)
    {
        var gamePreviews = await _repoFileSystemStorageHelper.GetAllAsync();

        gamePreviews.Add(gamePreview);

        await _repoFileSystemStorageHelper.SaveAllAsync(gamePreviews);

        return true;
    }

    public async Task<GamePreview?> GetGamePreviewAsync(Guid gameId)
    {
        var gamePreviews = await _repoFileSystemStorageHelper.GetAllAsync();
        return gamePreviews.SingleOrDefault(x => x.GameId == gameId);
    }

    public async Task<bool> UpdateGamePreviewAsync(GamePreview gamePreview)
    {
        var gamePreviews = await _repoFileSystemStorageHelper.GetAllAsync();

        var game = gamePreviews.SingleOrDefault(x => x.GameId == gamePreview.GameId);

        game.GradeId = gamePreview.GradeId;
        game.HomeTeam = gamePreview.HomeTeam;
        game.AwayTeam = gamePreview.AwayTeam;

        await _repoFileSystemStorageHelper.SaveAllAsync(gamePreviews);

        return true;
    }

    // public async Task<FileUpload> CreateAsync(FileUpload fileUpload)
    // {
    //     var fileUploads = await _repoFileSystemStorageHelper.GetAllAsync();

    //     fileUploads.Add(fileUpload);

    //     await _repoFileSystemStorageHelper.SaveAllAsync(fileUploads);

    //     return fileUpload;
    // }

    // public Task<bool> DeleteAsync(Guid id)
    // {
    //     throw new NotImplementedException();
    // }

    // public async Task<FileUpload?> GetAsync(Guid id)
    // {
    //     var clubs = await _repoFileSystemStorageHelper.GetAllAsync();
    //     return clubs.SingleOrDefault(x => x.Id == id);

    // }

    // public Task<FileUpload> UpdateAsync(FileUpload fileUpload)
    // {
    //     throw new NotImplementedException();
    // }
}