namespace Masaafa.WebApi.Models.Items;

public class UpdateItemGroupRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
}