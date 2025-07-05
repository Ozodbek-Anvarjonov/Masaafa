namespace Masaafa.WebApi.Models.SalesOrders;

public class UpdateSalesOrderItemRequest
{
    public Guid Id { get; set; }
    public Guid WarehouseItemId { get; set; }

    public decimal DiscountPercent { get; set; } = 0;
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }

    public DateTime? SentDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
}