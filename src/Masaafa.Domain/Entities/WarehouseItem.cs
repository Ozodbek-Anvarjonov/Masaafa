using Masaafa.Domain.Common.Entities;

namespace Masaafa.Domain.Entities;

public class WarehouseItem : SoftDeletedEntity
{
    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = default!;

    public Guid ItemId { get; set; }
    public Item Item { get; set; } = default!;

    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
}