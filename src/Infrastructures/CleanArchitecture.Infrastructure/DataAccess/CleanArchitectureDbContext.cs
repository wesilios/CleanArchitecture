using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.DataAccess;

public class CleanArchitectureDbContext : DbContext
{
    public CleanArchitectureDbContext(DbContextOptions<CleanArchitectureDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}