using FluentValidation;
using Masaafa.WebApi.Models.SalesOrders;

namespace Masaafa.WebApi.Validators.SalesOrders;

public class CreateSalesOrderRequestValidator : AbstractValidator<CreateSalesOrderRequest>
{
    public CreateSalesOrderRequestValidator()
    {
        RuleFor(entity => entity.SalesOrderNumber)
            .NotNull().NotEmpty().WithMessage("Sales order number cant be null or empty.");

        RuleFor(entity => entity.Address)
            .NotNull().NotEmpty().WithMessage("Address cant be null or empty.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.");
    }
}