using FluentValidation;
using Masaafa.WebApi.Models.Inventories;

namespace Masaafa.WebApi.Validators.Inventories;

public class UpdateInventoryRequestValidator : AbstractValidator<UpdateInventoryRequest>
{
    public UpdateInventoryRequestValidator()
    {
        RuleFor(entity => entity.InventoryNumber)
            .NotEmpty().NotNull().WithMessage("Inventory number cant be null or empty.");
    }
}