namespace Masaafa.WebApi.Models.Users;

public class CreateUserRequest
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string JobTitle { get; set; } = default!;

    public string CardCode { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;
}