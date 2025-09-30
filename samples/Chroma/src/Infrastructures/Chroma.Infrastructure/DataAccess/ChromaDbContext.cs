using Chroma.Domain.Entities;
using Chroma.Infrastructure.DataAccess.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Chroma.Infrastructure.DataAccess;

public class ChromaDbContext : DbContext
{
    public DbSet<Palette> Palettes { get; set; }
    
    public ChromaDbContext(DbContextOptions<ChromaDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PaletteConfigurations());
        
        base.OnModelCreating(modelBuilder);
    }
}