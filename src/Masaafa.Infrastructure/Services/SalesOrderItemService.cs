using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class SalesOrderItemService(IUnitOfWork unitOfWork, IUserContext userContext) : ISalesOrderItemService
{
    public async Task<PaginationResult<SalesOrderItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.SalesOrderItems.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<SalesOrderItem> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrderItems.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrderItem), nameof(SalesOrderItem.Id), id.ToString());

        return exist;
    }

    public async Task<SalesOrderItem> CreateAsync(SalesOrderItem item, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.SalesOrderItems.CreateAsync(item, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<SalesOrderItem> UpdateAsync(Guid id, SalesOrderItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrderItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrderItem), nameof(SalesOrderItem.Id), id.ToString());

        exist.WarehouseItemId = item.WarehouseItemId;
        exist.Note = item.Note;
        exist.DiscountPercent = item.DiscountPercent;
        exist.UnitPrice = item.UnitPrice;
        exist.Quantity = item.Quantity;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);

        await unitOfWork.SalesOrderItems.DeleteAsync(entity, saveChanges: true, cancellationToken: cancellationToken);

        return true;
    }
}