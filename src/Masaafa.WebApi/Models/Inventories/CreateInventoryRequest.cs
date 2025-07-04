namespace Masaafa.WebApi.Models.Inventories;

public class CreateInventoryRequest
{
    public string InventoryNumber { get; set; } = default!;
    public string? Note { get; set; }

    public Guid WarehouseId { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? CompletedDate { get; set; }
}