using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface IClientService
{
    Task<PaginationResult<Client>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<Client> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Client> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);

    Task<Client> CreateAsync(Client client, CancellationToken cancellationToken = default);

    Task<Client> UpdateAsync(Guid id, Client client, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}