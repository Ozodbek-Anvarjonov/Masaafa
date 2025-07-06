namespace Masaafa.WebApi.Models.SalesOrders;

public class UpdateSalesOrderItemRequest
{
    public Guid Id { get; set; }

    public string? Note { get; set; }

    public decimal DiscountPercent { get; set; } = 0;
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
}