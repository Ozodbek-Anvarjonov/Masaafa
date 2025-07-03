namespace Masaafa.WebApi.Models.Items;

public class ItemGroupResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
}