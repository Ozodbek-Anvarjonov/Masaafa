namespace Masaafa.WebApi.Models.Items;

public class ItemResponse
{
    public Guid Id { get; set; }

    public string ItemCode { get; set; } = default!;
    public string ItemName { get; set; } = default!;

    public Guid ItemGroupId { get; set; }
    public ItemGroupResponse ItemGroup { get; set; } = default!;

    public string Description { get; set; } = default!;
    public string UnitOfMeasure { get; set; } = default!;
    public decimal UnitPrice { get; set; } = default!;

    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public string Barcode { get; set; } = default!;
    public string Manufacturer { get; set; } = default!;
    public string Specifications { get; set; } = default!;
}