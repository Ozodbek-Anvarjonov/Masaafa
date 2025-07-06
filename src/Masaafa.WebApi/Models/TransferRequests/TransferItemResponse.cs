using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.Warehouses;

namespace Masaafa.WebApi.Models.TransferRequests;

public class TransferItemResponse
{
    public Guid Id { get; set; }

    public string? Note { get; set; }

    public Guid TransferRequestId { get; set; }
    public TransferResponse TransferRequest { get; set; } = default!;

    public Guid FromWarehouseItemId { get; set; }
    public WarehouseItemResponse FromWarehouseItem { get; set; } = default!;

    public Guid ToWarehouseItemId { get; set; }
    public WarehouseItemResponse ToWarehouseItem { get; set; } = default!;

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;

    public DateTimeOffset? SentDate { get; set; }
    public Guid? SendByUserId { get; set; }
    public Employee? SendByUser { get; set; }

    public DateTimeOffset? ReceivedDate { get; set; }
    public Guid? ReceivedByUserId { get; set; }
    public Employee? ReceivedByUser { get; set; }
}