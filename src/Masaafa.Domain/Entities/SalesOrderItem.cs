using Masaafa.Domain.Common.Entities;

namespace Masaafa.Domain.Entities;

public class SalesOrderItem : SoftDeletedEntity
{
    public string? Note { get; set; }

    public Guid SalesOrderId { get; set; }
    public SalesOrder SalesOrder { get; set; } = default!;

    public Guid WarehouseItemId { get; set; }
    public WarehouseItem WarehouseItem { get; set; } = default!;

    public decimal DiscountPercent { get; set; } = 0;
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity * (1 - DiscountPercent / 100);

    public DateTime? SentDate { get; set; }
    public Guid? SendByUserId { get; set; }
    public Employee? SendByUser { get; set; }

    public DateTime? ReceivedDate { get; set; }
    public Guid? ReceivedByUserId { get; set; }
    public Employee? ReceivedByUser { get; set; }
}