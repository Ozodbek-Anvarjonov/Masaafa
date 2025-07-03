using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class ItemGroupService(IUnitOfWork unitOfWork) : IItemGroupService
{
    public async Task<PaginationResult<ItemGroup>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.ItemGroups.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<ItemGroup> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.ItemGroups.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(ItemGroup), nameof(ItemGroup.Id), id.ToString());

        return entity;
    }

    public Task<ItemGroup> CreateAsync(ItemGroup item, CancellationToken cancellationToken = default)
    {
        var entity = unitOfWork.ItemGroups.CreateAsync(item, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<ItemGroup> UpdateAsync(Guid id, ItemGroup item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.ItemGroups.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(ItemGroup), nameof(ItemGroup.Id), id.ToString());

        exist.Name = item.Name;
        exist.Description = item.Description;

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id);

        _ = await unitOfWork.ItemGroups.DeleteAsync(entity, true, cancellationToken: cancellationToken);

        return true;
    }
}