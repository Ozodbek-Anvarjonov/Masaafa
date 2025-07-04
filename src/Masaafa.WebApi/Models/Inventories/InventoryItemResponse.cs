using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.Items;
using Masaafa.WebApi.Models.Users;

namespace Masaafa.WebApi.Models.Inventories;

public class InventoryItemResponse
{
    public Guid Id { get; set; }

    public string? Notes { get; set; }

    public Guid InventoryId { get; set; }
    public InventoryResponse Inventory { get; set; } = default!;

    public Guid ItemId { get; set; }
    public ItemResponse Item { get; set; } = default!;

    public decimal SystemQuantity { get; set; }
    public decimal ActualQuantity { get; set; }
    public decimal Difference => ActualQuantity - SystemQuantity;
    public string? Description { get; set; }

    public DateTimeOffset? CountedDate { get; set; }
    public Guid? CountedByUserId { get; set; }
    public EmployeeResponse? CountedByUser { get; set; }
}