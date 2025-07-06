namespace Masaafa.WebApi.Models.SalesOrders;

public class UpdateSalesOrderItemWarehouseRequest
{
    public Guid Id { get; set; }

    public Guid WarehouseItemId { get; set; }
}