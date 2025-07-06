using Masaafa.Domain.Enums;

namespace Masaafa.WebApi.Models.TransferRequests;

public class UpdateTransferRequest
{
    public Guid Id { get; set; }

    public string RequestNumber { get; set; } = default!;
    public string? Note { get; set; }
}