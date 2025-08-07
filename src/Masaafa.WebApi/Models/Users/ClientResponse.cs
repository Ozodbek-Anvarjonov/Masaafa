namespace Masaafa.WebApi.Models.Users;

public class ClientResponse
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string CardCode { get; set; } = default!;

    public decimal Balance { get; set; } = default!;

    public long TelegramId { get; set; } = default!;
}