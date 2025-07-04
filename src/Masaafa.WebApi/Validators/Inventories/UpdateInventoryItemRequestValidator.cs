using FluentValidation;
using Masaafa.WebApi.Models.Inventories;

namespace Masaafa.WebApi.Validators.Inventories;

public class UpdateInventoryItemRequestValidator : AbstractValidator<UpdateInventoryItemRequest>
{
    public UpdateInventoryItemRequestValidator()
    {
        RuleFor(entity => entity.ActualQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cant be lower than 0.");
    }
}