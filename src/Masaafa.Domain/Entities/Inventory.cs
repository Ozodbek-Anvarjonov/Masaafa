using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class Inventory : SoftDeletedEntity
{
    public string InventoryNumber { get; set; } = default!;
    public string? Note { get; set; }

    public DateTime InventoryDate { get; set; } = DateTime.Now;

    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = default!;

    public InventoryStatus Status { get; set; } = InventoryStatus.NotStarted;

    public Guid CreatedByUserId { get; set; }
    public Employee CreatedByUser { get; set; }

    public DateTime? StartedDate { get; set; }
    public Guid? StartedByUserId { get; set; }
    public Employee? StartedByUser { get; set; }

    public DateTime? CompletedDate { get; set; }
    public Guid? CompletedByUserId { get; set; }
    public Employee? CompletedByUser { get; set; }

    public IEnumerable<InventoryItem> Items { get; set; }
}