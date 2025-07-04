namespace Masaafa.WebApi.Models.Inventories;

public class UpdateInventoryRequest
{
    public Guid Id { get; set; }

    public string InventoryNumber { get; set; } = default!;

    public string? Note { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? CompletedDate { get; set; }
}