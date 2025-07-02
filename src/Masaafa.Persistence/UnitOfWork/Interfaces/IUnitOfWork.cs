using Masaafa.Persistence.Repositories.Interfaces;

namespace Masaafa.Persistence.UnitOfWork.Interfaces;

public interface IUnitOfWork : ITransactionManager, IDisposable, IAsyncDisposable
{
    IClientRepository Clients { get; }

    IEmployeeRepository Employees { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}