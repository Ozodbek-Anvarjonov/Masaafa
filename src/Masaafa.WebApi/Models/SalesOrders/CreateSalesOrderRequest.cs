namespace Masaafa.WebApi.Models.SalesOrders;

public class CreateSalesOrderRequest
{
    public string SalesOrderNumber { get; set; } = default!;
    public string? Note { get; set; }

    public DateTimeOffset DocDate { get; set; }
    public DateTimeOffset DocDueDate { get; set; }

    public Guid ClientId { get; set; }

    public string Address { get; set; } = default!;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}