using Masaafa.WebApi.Models.Users;
using Masaafa.WebApi.Models.Warehouses;

namespace Masaafa.WebApi.Models.Inventories;

public class InventoryItemResponse
{
    public Guid Id { get; set; }

    public string? Notes { get; set; }

    public Guid InventoryId { get; set; }
    public InventoryResponse Inventory { get; set; } = default!;

    public Guid WarehouseItemId { get; set; }
    public WarehouseItemResponse WarehouseItem { get; set; } = default!;

    public decimal SystemQuantity { get; set; }
    public decimal ActualQuantity { get; set; }
    public decimal Difference => ActualQuantity - SystemQuantity;
    public string? Description { get; set; }

    public DateTimeOffset? CountedDate { get; set; }
    public Guid? CountedByUserId { get; set; }
    public EmployeeResponse? CountedByUser { get; set; }
}