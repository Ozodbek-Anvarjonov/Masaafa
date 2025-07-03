using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface IItemService
{
    Task<PaginationResult<Item>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        );

    Task<PaginationResult<Item>> GetByGroupIdAsync(
        Guid groupId,
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        );

    Task<Item> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Item> CreateAsync(Item item, CancellationToken cancellationToken = default);

    Task<Item> UpdateAsync(Guid id, Item item, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}