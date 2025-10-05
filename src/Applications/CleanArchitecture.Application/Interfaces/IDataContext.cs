namespace CleanArchitecture.Application.Interfaces;

public interface IDataContext : IDisposable
{
    // Method to commit all changes tracked in the current unit of work
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    // Optional: Method to start and manage a database transaction
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    // Optional: Method to commit the current transaction
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    // Optional: Method to rollback the current transaction
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}