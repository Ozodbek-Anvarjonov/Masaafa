namespace Masaafa.WebApi.Models.SalesOrders;

public class UpdateSalesOrderItemSendDate
{
    public Guid Id { get; set; }

    public DateTimeOffset SentDate { get; set; }
}