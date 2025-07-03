using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class ItemService(IUnitOfWork unitOfWork) : IItemService
{
    public async Task<PaginationResult<Item>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.Items.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<PaginationResult<Item>> GetByGroupIdAsync(
        Guid groupId,
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.Items.GetByGroupIdAsync(groupId, @params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<Item> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Items.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Item), nameof(Item.Id), id.ToString());

        return entity;
    }

    public Task<Item> CreateAsync(Item item, CancellationToken cancellationToken = default)
    {
        var entity = unitOfWork.Items.CreateAsync(item, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<Item> UpdateAsync(Guid id, Item item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Items.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Item), nameof(Item.Id), id.ToString());

        exist.ItemCode = item.ItemCode;
        exist.ItemName = item.ItemName;

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id);

        _ = await unitOfWork.Items.DeleteAsync(entity, true, cancellationToken: cancellationToken);

        return true;
    }
}