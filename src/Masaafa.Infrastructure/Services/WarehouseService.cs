using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class WarehouseService(IUnitOfWork unitOfWork) : IWarehouseService
{
    public async Task<PaginationResult<Warehouse>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.Warehouses.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<Warehouse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Warehouses.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Warehouse), nameof(Warehouse.Id), id.ToString());

        return entity;
    }

    public Task<Warehouse> CreateAsync(Warehouse item, CancellationToken cancellationToken = default)
    {
        var entity = unitOfWork.Warehouses.CreateAsync(item, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<Warehouse> UpdateAsync(Guid id, Warehouse item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Warehouses.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Warehouse), nameof(Warehouse.Id), id.ToString());

        exist.Name = item.Name;
        exist.Code = item.Code;
        exist.Address = item.Address;
        exist.IsActive = item.IsActive;

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id);

        _ = await unitOfWork.Warehouses.DeleteAsync(entity, true, cancellationToken: cancellationToken);

        return true;
    }
}