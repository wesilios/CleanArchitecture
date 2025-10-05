using System.Data;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchitecture.Infrastructure.DataAccess;

public class CleanArchitectureDbContext : DbContext
{
    public DbSet<Palette> Palettes { get; set; }

    private IDbContextTransaction? _currentTransaction;

    public CleanArchitectureDbContext(DbContextOptions<CleanArchitectureDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PaletteConfigurations());

        base.OnModelCreating(modelBuilder);
    }

    // IDataContext Transaction Management
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            return;
        }

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            await _currentTransaction?.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _currentTransaction == null ? Task.CompletedTask : _currentTransaction.RollbackAsync(cancellationToken);
    }
}