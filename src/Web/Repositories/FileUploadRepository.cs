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
