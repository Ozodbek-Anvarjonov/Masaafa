using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class InventoryService(IUnitOfWork unitOfWork, IUserContext userContext) : IInventoryService
{
    public async Task<PaginationResult<Inventory>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.Inventories.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<Inventory> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Inventories.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Inventory), nameof(Inventory.Id), id.ToString());

        return entity;
    }

    public async Task<Inventory> CreateAsync(Inventory inventory, CancellationToken cancellationToken = default)
    {
        inventory.CreatedByUserId = userContext.GetRequiredUserId();
        inventory.InventoryDate = DateTimeOffset.UtcNow;

        if (inventory.StartedDate is not null)
        {
            inventory.StartedByUserId = userContext.GetRequiredUserId();
            inventory.Status = InventoryStatus.InProgress;
        }

        if (inventory.CompletedDate is not null)
        {
            inventory.CompletedByUserId = userContext.GetRequiredUserId();
            inventory.Status = InventoryStatus.Completed;
        }

        var entity = await unitOfWork.Inventories.CreateAsync(inventory, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<Inventory> UpdateAsync(Guid id, Inventory inventory, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Inventories.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Inventory), nameof(Inventory.Id), id.ToString());

        exist.InventoryNumber = inventory.InventoryNumber;
        exist.Note = inventory.Note;

        if (inventory.StartedDate is not null)
        {
            exist.StartedByUserId = userContext.GetRequiredUserId();
            exist.StartedDate = inventory.StartedDate; 
            exist.Status = InventoryStatus.InProgress;
        }

        if (inventory.CompletedDate is not null)
        {
            exist.StartedByUserId = userContext.GetRequiredUserId();
            exist.StartedDate = inventory.CompletedDate;
            exist.Status = InventoryStatus.Completed;
        }

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await GetByIdAsync(id, cancellationToken: cancellationToken);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        await unitOfWork.Inventories.DeleteAsync(exist);
        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return true;
    }
}