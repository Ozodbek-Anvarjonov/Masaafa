using FluentValidation;
using Masaafa.WebApi.Models.Warehouses;

namespace Masaafa.WebApi.Validators.Warehouses;

public class UpdateWarehouseItemRequestValidator : AbstractValidator<UpdateWarehouseItemRequest>
{
    public UpdateWarehouseItemRequestValidator()
    {
        RuleFor(entity => entity.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("WarehouseItem's item's quantity cant be lower than 0.");

        RuleFor(entity => entity.ReservedQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("WarehouseItem's item's reserved quantity cant be lower than 0.");
    }
}