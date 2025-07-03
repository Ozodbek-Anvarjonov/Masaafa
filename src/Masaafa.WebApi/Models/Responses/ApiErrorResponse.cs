namespace Masaafa.WebApi.Models.Responses;

public class ApiErrorResponse
{
    public string Type { get; set; } = default!;

    public int Status { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Detail { get; set; } = default!;

    public IDictionary<string, string[]>? Errors { get; set; }
}