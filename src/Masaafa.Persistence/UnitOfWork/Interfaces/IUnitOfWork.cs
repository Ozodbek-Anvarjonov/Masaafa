namespace Masaafa.Persistence.UnitOfWork.Interfaces;

public interface IUnitOfWork : ITransactionManager, IDisposable, IAsyncDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}