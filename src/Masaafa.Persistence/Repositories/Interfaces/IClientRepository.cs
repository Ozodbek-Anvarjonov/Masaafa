using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface IClientRepository
{
    IQueryable<Client> Get();

    Task<PaginationResult<Client>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task<Client?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<Client?> GetByPhoneNumberAsync(string phoneNumber, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<Client> CreateAsync(Client client, bool saveChanges = false, CancellationToken cancellationToken = default);
    
    Task<Client> UpdateAsync(Client client, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<Client> DeleteAsync(Client client, bool saveChanges = false, CancellationToken cancellationToken = default);
}