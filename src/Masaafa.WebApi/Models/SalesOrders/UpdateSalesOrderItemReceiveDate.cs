namespace Masaafa.WebApi.Models.SalesOrders;

public class UpdateSalesOrderItemReceiveDate
{
    public Guid Id { get; set; }

    public DateTimeOffset ReceivedDate { get; set; }
}