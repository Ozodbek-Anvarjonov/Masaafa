namespace Masaafa.WebApi.Models.Users;

public class UpdateEmployeeRequest
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string SalesPersonCode { get; set; } = default!;
}