using Masaafa.Domain.Enums;

namespace Masaafa.WebApi.Models.TransferRequests;

public class CreateTransferRequest
{
    public string RequestNumber { get; set; } = default!;
    public string? Note { get; set; }

    public Guid FromWarehouseId { get; set; }
    public Guid ToWarehouseId { get; set; }
}