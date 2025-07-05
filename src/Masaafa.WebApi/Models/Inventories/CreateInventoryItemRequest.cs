namespace Masaafa.WebApi.Models.Inventories;

public class CreateInventoryItemRequest
{
    public string? Notes { get; set; }

    public Guid InventoryId { get; set; }

    public Guid WarehouseItemId { get; set; }

    public decimal ActualQuantity { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset CountedDate { get; set; }
}