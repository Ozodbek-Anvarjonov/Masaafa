namespace Masaafa.WebApi.Models.Inventories;

public class UpdateInventoryItemRequest
{
    public Guid Id { get; set; }

    public Guid ItemId { get; set; }

    public decimal ActualQuantity { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset CountedDate { get; set; }
}