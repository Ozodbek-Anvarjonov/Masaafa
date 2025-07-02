using Masaafa.Domain.Enums;
using System.Text.Json.Serialization;

namespace Masaafa.WebApi.Models.Users;

public class UserResponse
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string JobTitle { get; set; } = default!;

    public string CardCode { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; } = default!;
}