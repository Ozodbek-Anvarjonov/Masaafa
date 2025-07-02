namespace Masaafa.WebApi.Models.Users;

public class CreateEmployeeRequest
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string SalesPersonCode { get; set; } = default!;
}