namespace Masaafa.WebApi.Models.SalesOrders;

public class UpdateSalesOrderApprovedRequest
{
    public Guid Id { get; set; }

    public DateTimeOffset ApprovedDate { get; set; }
}