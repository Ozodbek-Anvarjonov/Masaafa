using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface IItemGroupService
{
    Task<PaginationResult<ItemGroup>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        );

    Task<ItemGroup> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ItemGroup> CreateAsync(ItemGroup group, CancellationToken cancellationToken = default);

    Task<ItemGroup> UpdateAsync(Guid id, ItemGroup group, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}