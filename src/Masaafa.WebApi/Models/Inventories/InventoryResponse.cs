using Masaafa.Domain.Enums;
using Masaafa.WebApi.Models.Users;
using Masaafa.WebApi.Models.Warehouses;
using System.Text.Json.Serialization;

namespace Masaafa.WebApi.Models.Inventories;

public class InventoryResponse
{
    public Guid Id { get; set; }

    public string InventoryNumber { get; set; } = default!;
    public string? Note { get; set; }

    public Guid WarehouseId { get; set; }
    public WarehouseResponse Warehouse { get; set; } = default!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public InventoryStatus Status { get; set; } = InventoryStatus.NotStarted;

    public DateTime InventoryDate { get; set; }
    public Guid CreatedByUserId { get; set; }
    public EmployeeResponse CreatedByUser { get; set; } = default!;

    public DateTime? StartedDate { get; set; }
    public Guid? StartedByUserId { get; set; }
    public EmployeeResponse? StartedByUser { get; set; }

    public DateTime? CompletedDate { get; set; }
    public Guid? CompletedByUserId { get; set; }
    public EmployeeResponse? CompletedByUser { get; set; }
}