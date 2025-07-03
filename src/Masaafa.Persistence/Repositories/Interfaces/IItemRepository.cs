using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface IItemRepository
{
    IQueryable<Item> Get();

    Task<PaginationResult<Item>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<PaginationResult<Item>> GetByGroupIdAsync(
        Guid groupId,
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<Item?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<Item> CreateAsync(Item item, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<Item> UpdateAsync(Item item, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<Item> DeleteAsync(Item item, bool saveChanges = false, CancellationToken cancellationToken = default);
}