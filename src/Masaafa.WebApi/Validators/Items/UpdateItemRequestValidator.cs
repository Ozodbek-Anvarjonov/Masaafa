using FluentValidation;
using Masaafa.WebApi.Models.Items;

namespace Masaafa.WebApi.Validators.Items;

public class UpdateItemRequestValidator : AbstractValidator<UpdateItemRequest>
{
    public UpdateItemRequestValidator()
    {
        RuleFor(entity => entity.ItemCode)
            .NotNull().NotEmpty().WithMessage("WarehouseItem's code cant be null or empty.");

        RuleFor(entity => entity.ItemName)
            .NotNull().NotEmpty().WithMessage("WarehouseItem's name cant be null or empty.");

        RuleFor(entity => entity.Description)
            .NotNull().NotEmpty().WithMessage("WarehouseItem's description cant be null or empty.");

        RuleFor(entity => entity.UnitOfMeasure)
            .NotNull().NotEmpty().WithMessage("WarehouseItem's unit of measure cant be null or empty.");

        RuleFor(entity => entity.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("WarehouseItem's unit price cant be lower than 0.");

        RuleFor(entity => entity.Barcode)
            .NotNull().NotEmpty().WithMessage("WarehouseItem's barcode cant be null or empty.")
            .MaximumLength(15);

        RuleFor(entity => entity.Manufacturer)
            .NotNull().NotEmpty().WithMessage("WarehouseItem's manufacturer cant be null or empty.")
            .MaximumLength(15);

        RuleFor(entity => entity.Specifications)
            .NotNull().NotEmpty().WithMessage("WarehouseItem's specifications cant be null or empty.")
            .MaximumLength(15);
    }   
}