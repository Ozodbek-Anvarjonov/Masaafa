namespace Masaafa.WebApi.Models.SalesOrders;

public class UpdateSalesOrderItemRequest
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid WarehouseId { get; set; }

    public decimal DiscountPercent { get; set; } = 0;
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }

    public decimal? SentQuantity { get; set; }
    public decimal? ReceivedQuantity { get; set; }
    public DateTime? SentDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
}