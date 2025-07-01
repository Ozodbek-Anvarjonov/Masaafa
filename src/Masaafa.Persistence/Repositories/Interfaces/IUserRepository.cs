using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface IUserRepository
{
    IQueryable<User> Get();

    Task<PaginationResult<User>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<User> CreateAsync(User user, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<User> UpdateAsync(User user, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<User> DeleteAsync(User User, bool saveChanges = false, CancellationToken cancellationToken = default);
}