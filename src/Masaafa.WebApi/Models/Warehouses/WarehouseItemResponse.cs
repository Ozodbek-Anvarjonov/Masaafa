using Masaafa.WebApi.Models.Items;

namespace Masaafa.WebApi.Models.Warehouses;

public class WarehouseItemResponse
{
    public Guid Id { get; set; }

    public Guid WarehouseId { get; set; }
    public WarehouseResponse Warehouse { get; set; } = default!;

    public Guid ItemId { get; set; }
    public ItemResponse Item { get; set; } = default!;

    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
}