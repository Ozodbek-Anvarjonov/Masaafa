namespace Masaafa.WebApi.Models.Warehouses;

public class CreateWarehouseItemRequest
{
    public Guid WarehouseId { get; set; }

    public Guid ItemId { get; set; }

    public decimal Quantity { get; set; }

    public decimal ReservedQuantity { get; set; }
}