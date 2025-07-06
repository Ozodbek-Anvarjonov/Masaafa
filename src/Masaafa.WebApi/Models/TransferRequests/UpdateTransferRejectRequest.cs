namespace Masaafa.WebApi.Models.TransferRequests;

public class UpdateTransferRejectRequest
{
    public Guid Id { get; set; }

    public DateTimeOffset RejectionDate { get; set; }

    public string RejectionReason { get; set; } = default!;
}