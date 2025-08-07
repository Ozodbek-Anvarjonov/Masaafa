namespace Masaafa.Domain.Entities;

public class Client : User
{
    public string CardCode { get; set; } = default!;

    public decimal Balance { get; set; } = default!;
    
    public long TelegramId { get; set; } = default!;
}