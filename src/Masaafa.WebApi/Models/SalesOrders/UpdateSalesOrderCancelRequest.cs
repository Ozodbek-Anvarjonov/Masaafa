namespace Masaafa.WebApi.Models.SalesOrders;

public class UpdateSalesOrderCancelRequest
{
    public Guid Id { get; set; }

    public DateTimeOffset CancelledDate { get; set; }

    public string CancellationReason { get; set; } = default!;
}