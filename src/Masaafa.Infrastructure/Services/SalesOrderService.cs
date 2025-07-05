using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class SalesOrderService(IUnitOfWork unitOfWork, IUserContext userContext) : ISalesOrderService
{
    public async Task<PaginationResult<SalesOrder>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.SalesOrders.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<SalesOrder> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrders.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), nameof(SalesOrder.Id), id.ToString());

        return exist;
    }

    public async Task<SalesOrder> CreateAsync(SalesOrder order, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.SalesOrders.CreateAsync(order, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<SalesOrder> UpdateAsync(Guid id, SalesOrder order, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrders.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), nameof(SalesOrder.Id), id.ToString());

        exist.SalesOrderNumber = order.SalesOrderNumber;
        exist.Note = order.Note;
        exist.DocDate = order.DocDate;
        exist.DocDueDate = order.DocDueDate;
        exist.ClientId = order.ClientId;
        exist.Address = order.Address;
        exist.Latitude = order.Latitude;
        exist.Longitude = order.Longitude;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        await unitOfWork.SalesOrders.DeleteAsync(entity, cancellationToken: cancellationToken);
        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return true;
    }
}