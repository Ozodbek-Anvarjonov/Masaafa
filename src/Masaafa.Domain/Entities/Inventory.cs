using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class Inventory : SoftDeletedEntity
{
    public string InventoryNumber { get; set; } = default!;
    public string? Note { get; set; }

    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = default!;

    public InventoryStatus Status { get; set; } = InventoryStatus.NotStarted;

    public DateTimeOffset InventoryDate { get; set; }
    public Guid CreatedByUserId { get; set; }
    public Employee CreatedByUser { get; set; } = default!;

    public DateTimeOffset? StartedDate { get; set; }
    public Guid? StartedByUserId { get; set; }
    public Employee? StartedByUser { get; set; }

    public DateTimeOffset? CompletedDate { get; set; }
    public Guid? CompletedByUserId { get; set; }
    public Employee? CompletedByUser { get; set; }
}