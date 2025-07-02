using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class Inventory : SoftDeletedEntity
{
    public string InventoryNumber { get; set; } = default!;
    public DateTime InventoryDate { get; set; } = DateTime.Now;

    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = default!;

    public InventoryStatus Status { get; set; } = InventoryStatus.NotStarted;

    public Guid CreatedByUserId { get; set; }
    public Employee CreatedBy { get; set; }

    public DateTime? StartedDate { get; set; }

    public int? StartedByUserId { get; set; }
    public Employee StartedBy { get; set; } = default!;

    public DateTime? CompletedDate { get; set; }
    public Guid? CompletedByUserId { get; set; }
    public Employee CompletedBy { get; set; } = default!;

    public IEnumerable<InventoryItem> Items { get; set; }
}