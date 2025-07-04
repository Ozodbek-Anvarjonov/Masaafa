using FluentValidation;
using Masaafa.WebApi.Models.Inventories;

namespace Masaafa.WebApi.Validators.Inventories;

public class CreateInventoryRequestValidator : AbstractValidator<CreateInventoryRequest>
{
    public CreateInventoryRequestValidator()
    {
        RuleFor(entity => entity.InventoryNumber)
            .NotEmpty().NotNull().WithMessage("Inventory number cant be null or empty.");
    }
}