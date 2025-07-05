namespace Masaafa.WebApi.Models.Warehouses;

public class CreateWarehouseRequest
{
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string Address { get; set; } = default!;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public bool IsActive { get; set; } = true;
}