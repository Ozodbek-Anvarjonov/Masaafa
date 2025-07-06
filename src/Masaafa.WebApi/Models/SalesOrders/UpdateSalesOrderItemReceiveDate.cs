namespace Masaafa.WebApi.Models.SalesOrders;

public class UpdateSalesOrderItemReceiveDate
{
    public Guid Id { get; set; }

    public DateTimeOffset ReceivedByUser { get; set; }
}