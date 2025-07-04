using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.WebApi.Models.Users;
using Masaafa.WebApi.Models.Warehouses;
using System.Text.Json.Serialization;

namespace Masaafa.WebApi.Models.TransferRequests;

public class TransferResponse
{
    public Guid Id { get; set; }

    public string RequestNumber { get; set; } = default!;
    public string? Note { get; set; }

    public DateTime RequestDate { get; set; }

    public Guid FromWarehouseId { get; set; }
    public WarehouseResponse FromWarehouse { get; set; } = default!;

    public Guid ToWarehouseId { get; set; }
    public WarehouseResponse ToWarehouse { get; set; } = default!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransferRequestStatus Status { get; set; } = TransferRequestStatus.New;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransferProcessStatus ProcessStatus { get; set; } = TransferProcessStatus.OnProcess;

    public Guid CreatedByUserId { get; set; }
    public EmployeeResponse CreatedByUser { get; set; } = default!;

    public DateTime? ApprovedDate { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public EmployeeResponse? ApprovedByUser { get; set; }

    public DateTime? RejectedDate { get; set; }
    public Guid? RejectedByUserId { get; set; }
    public EmployeeResponse? RejectedByUser { get; set; }
    public string? RejectionReason { get; set; }
}