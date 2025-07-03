using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface IItemGroupRepository
{
    IQueryable<ItemGroup> Get();

    Task<PaginationResult<ItemGroup>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task<ItemGroup?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<ItemGroup> CreateAsync(ItemGroup itemGroup, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<ItemGroup> UpdateAsync(ItemGroup itemGroup, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<ItemGroup> DeleteAsync(ItemGroup itemGroup, bool saveChanges = false, CancellationToken cancellationToken = default);
}