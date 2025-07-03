using FluentValidation;
using Masaafa.WebApi.Models.Warehouses;

namespace Masaafa.WebApi.Validators.Warehouses;

public class CreateWarehouseRequestValidator : AbstractValidator<CreateWarehouseRequest>
{
    public CreateWarehouseRequestValidator()
    {
        RuleFor(entity => entity.Name)
            .NotEmpty().NotNull().WithMessage("Name of warehouse can't be null or empty")
            .MaximumLength(100).WithMessage("Name of warehouse's length cant be longer than 100.");

        RuleFor(entity => entity.Code)
            .NotEmpty().NotNull().WithMessage("Warehouse's code cant be null or empty")
            .MaximumLength(15).WithMessage("Warehouse's code's length cant be longer than 15.");

        RuleFor(entity => entity.Address)
            .NotNull().NotEmpty().WithMessage("Warehouse's address cant be null or empty")
            .MaximumLength(100).WithMessage("Warehouse's address's length cant be longer than 100");
    }
}