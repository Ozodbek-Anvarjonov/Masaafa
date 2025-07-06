namespace Masaafa.WebApi.Models.SalesOrders;

public class UpdateSalesOrderRejectRequest
{
    public Guid Id { get; set; }

    public DateTimeOffset RejectedDate { get; set; }

    public string RejectionReason { get; set; } = default!;
}