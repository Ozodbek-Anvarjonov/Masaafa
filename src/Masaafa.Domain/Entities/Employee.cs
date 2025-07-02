namespace Masaafa.Domain.Entities;

public class Employee : User
{
    public string SalesPersonCode { get; set; } = default!;
}