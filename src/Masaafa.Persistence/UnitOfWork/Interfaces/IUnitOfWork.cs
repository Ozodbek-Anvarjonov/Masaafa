using Masaafa.Persistence.Repositories.Interfaces;

namespace Masaafa.Persistence.UnitOfWork.Interfaces;

public interface IUnitOfWork : ITransactionManager, IDisposable, IAsyncDisposable
{
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}