using FluentValidation;
using Masaafa.WebApi.Models.Users;

namespace Masaafa.WebApi.Validators.Users;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator()
    {
        RuleFor(entity => entity.FirstName)
            .NotEmpty().NotNull().WithMessage("User's name cant be null or empty.");

        RuleFor(entity => entity.LastName)
            .NotEmpty().NotNull().WithMessage("User's last name cant be null or empty.");

        RuleFor(entity => entity.PhoneNumber)
            .NotEmpty().NotNull().WithMessage("User's phone number cant be null or empty.")
            .Matches(@"^(\+998|998)?\d{9}$").WithMessage("Invalid phone number format.");

        RuleFor(entity => entity.SalesPersonCode)
            .NotEmpty().NotNull().WithMessage("User's card code cant be null or empty.");
    }
}