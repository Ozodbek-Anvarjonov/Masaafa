namespace Masaafa.WebApi.Models.Warehouses;

public class WarehouseResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string Address { get; set; } = default!;

    public bool IsActive { get; set; } = true;
}