using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class SalesOrder : SoftDeletedEntity
{
    public TransferRequestStatus Status { get; set; } = TransferRequestStatus.New;
}