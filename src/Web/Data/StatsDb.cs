namespace STKBC.Stats.Data;

using Microsoft.EntityFrameworkCore;
using STKBC.Stats.Data.Models;

public class StatsDb : DbContext
{
    public StatsDb(DbContextOptions<StatsDb> options)
        : base(options) { }

    public DbSet<FileUpload> FileUploads => Set<FileUpload>();
    
}