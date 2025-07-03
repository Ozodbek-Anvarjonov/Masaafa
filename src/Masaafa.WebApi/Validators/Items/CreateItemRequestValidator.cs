using FluentValidation;
using Masaafa.WebApi.Models.Items;

namespace Masaafa.WebApi.Validators.Items;

public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
{
    public CreateItemRequestValidator()
    {
        RuleFor(entity => entity.ItemCode)
            .NotNull().NotEmpty().WithMessage("Item's code cant be null or empty.");

        RuleFor(entity => entity.ItemName)
            .NotNull().NotEmpty().WithMessage("Item's name cant be null or empty.");

        RuleFor(entity => entity.Description)
            .NotNull().NotEmpty().WithMessage("Item's description cant be null or empty.");

        RuleFor(entity => entity.UnitOfMeasure)
            .NotNull().NotEmpty().WithMessage("Item's unit of measure cant be null or empty.");

        RuleFor(entity => entity.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Item's unit price cant be lower than 0.");

        RuleFor(entity => entity.Barcode)
            .NotNull().NotEmpty().WithMessage("Item's barcode cant be null or empty.")
            .MaximumLength(15);

        RuleFor(entity => entity.Manufacturer)
            .NotNull().NotEmpty().WithMessage("Item's manufacturer cant be null or empty.")
            .MaximumLength(15);

        RuleFor(entity => entity.Specifications)
            .NotNull().NotEmpty().WithMessage("Item's specifications cant be null or empty.")
            .MaximumLength(15);
    }
}