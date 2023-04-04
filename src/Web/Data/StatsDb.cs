namespace STKBC.Stats.Data;

using Microsoft.EntityFrameworkCore;

class StatsDb : DbContext
{
    public StatsDb(DbContextOptions<StatsDb> options)
        : base(options) { }

    // public DbSet<Todo> Todos => Set<Todo>();
    
}