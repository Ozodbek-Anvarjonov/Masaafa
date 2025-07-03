using Masaafa.Domain.Common.Entities;

namespace Masaafa.Domain.Entities;

public class InventoryItem : SoftDeletedEntity
{
    public string? Notes { get; set; }

    public Guid InventoryId { get; set; }
    public Inventory Inventory { get; set; } = default!;

    public Guid ItemId { get; set; }
    public Item Item { get; set; } = default!;

    public decimal SystemQuantity { get; set; }
    public decimal ActualQuantity { get; set; }
    public decimal Difference => ActualQuantity - SystemQuantity;

    public DateTimeOffset? CountedDate { get; set; }
    public Guid? CountedByUserId { get; set; }
    public Employee? CountedByUser { get; set; }
}