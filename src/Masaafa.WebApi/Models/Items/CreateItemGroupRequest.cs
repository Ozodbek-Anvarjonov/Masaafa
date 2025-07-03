namespace Masaafa.WebApi.Models.Items;

public class CreateItemGroupRequest
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
}