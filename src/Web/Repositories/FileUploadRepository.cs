using STKBC.Stats.Data;
using STKBC.Stats.Data.Models;

namespace STKBC.Stats.Repositories;


public interface IFileUploadRepository
{
    Task<FileUpload> CreateAsync(FileUpload fileUpload);
    Task<FileUpload?> GetAsync(Guid id);
    Task<FileUpload> UpdateAsync(FileUpload fileUpload);
    Task<bool> DeleteAsync(Guid id);
}


public class InMemoryFileUploadRepository : IFileUploadRepository
{
    private List<FileUpload> _fileUploads;

    public InMemoryFileUploadRepository(List<FileUpload>? fileUploads = null)
    {
        _fileUploads = fileUploads ?? new List<FileUpload>();
    }

    public async Task<FileUpload> CreateAsync(FileUpload fileUpload)
    {
        _fileUploads.Add(fileUpload);
        return fileUpload;
    }

    public async Task<FileUpload?> GetAsync(Guid id)
    {
        return _fileUploads.FirstOrDefault(x => x.Id == id);
    }

    public async Task<FileUpload> UpdateAsync(FileUpload fileUpload)
    {
        var existingFileUpload = _fileUploads.FirstOrDefault(x => x.Id == fileUpload.Id);
        if (existingFileUpload == null)
        {
            throw new Exception("FileUpload not found");
        }
        existingFileUpload.Name = fileUpload.Name;
        existingFileUpload.Location = fileUpload.Location;
        existingFileUpload.Size = fileUpload.Size;
        existingFileUpload.Extension = fileUpload.Extension;
        existingFileUpload.Hash = fileUpload.Hash;

        return existingFileUpload;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var fileUpload = _fileUploads.FirstOrDefault(x => x.Id == id);
        if (fileUpload == null)
        {
            return false;
        }
        _fileUploads.Remove(fileUpload);
        return true;
    }
}


public class LocalStorageFileUploadRepository : IFileUploadRepository
{
    private readonly RepoFileSystemStorage<FileUpload> _repoFileSystemStorageHelper;

    public LocalStorageFileUploadRepository(RepoFileSystemStorageHelper storageHelper)
    {
        _repoFileSystemStorageHelper = storageHelper.GetRepoFileSystemStorage<FileUpload>();
    }

    public async Task<FileUpload> CreateAsync(FileUpload fileUpload)
    {
        var fileUploads = await _repoFileSystemStorageHelper.GetAllAsync();

        fileUploads.Add(fileUpload);

        await _repoFileSystemStorageHelper.SaveAllAsync(fileUploads);

        return fileUpload;
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<FileUpload?> GetAsync(Guid id)
    {
        var clubs = await _repoFileSystemStorageHelper.GetAllAsync();
        return clubs.SingleOrDefault(x => x.Id == id);

    }

    public Task<FileUpload> UpdateAsync(FileUpload fileUpload)
    {
        throw new NotImplementedException();
    }

    // public async Task<List<FileUpload>> GetClubsAsync()
    // {
    //     return await _repoFileSystemStorageHelper.GetAllAsync();
    // }

    // public async Task<FileUpload?> GetClubAsync(Guid? id)
    // {
    //     var clubs = await _repoFileSystemStorageHelper.GetAllAsync();
    //     return clubs.SingleOrDefault(x => x.Id == id);
    // }

    // public async Task CreateClubAsync(Club? club)
    // {
    //     if (club == null)
    //     {
    //         throw new ArgumentNullException(nameof(club));
    //     }

    //     var clubs = await _repoFileSystemStorageHelper.GetAllAsync();

    //     clubs.Add(club);

    //     await _repoFileSystemStorageHelper.SaveAllAsync(clubs);
    // }
}




public class FileUploadRepository : IFileUploadRepository
{
    private readonly StatsDb _context;

    public FileUploadRepository(StatsDb context)
    {
        _context = context;
    }

    public async Task<FileUpload> CreateAsync(FileUpload fileUpload)
    {
        await _context.FileUploads.AddAsync(fileUpload);
        await _context.SaveChangesAsync();
        return fileUpload;
    }

    public async Task<FileUpload?> GetAsync(Guid id)
    {
        return await _context.FileUploads.FindAsync(id);
    }

    public async Task<FileUpload> UpdateAsync(FileUpload fileUpload)
    {
        _context.FileUploads.Update(fileUpload);
        await _context.SaveChangesAsync();
        return fileUpload;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var fileUpload = await GetAsync(id);

        if (fileUpload == null)
        {
            return false;
        }
        _context.FileUploads.Remove(fileUpload);
        await _context.SaveChangesAsync();

        return true;
    }
}
