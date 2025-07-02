using Masaafa.Domain.Common.Entities;

namespace Masaafa.Domain.Entities;

public class Item : SoftDeletedEntity
{
    public string ItemCode { get; set; } = default!;
    public string ItemName { get; set; } = default!;

    public Guid ItemGroupId { get; set; }
    public ItemGroup ItemGroup { get; set; } = default!;

    public string Description { get; set; } = default!;
    public string UnitOfMeasure { get; set; } = default!;
    public decimal Price { get; set; } = default!;

    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public string Barcode { get; set; } = default!;
    public string Manufacturer { get; set; } = default!;
    public string Specifications { get; set; } = default!;
}