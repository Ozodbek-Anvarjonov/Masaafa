using FluentValidation;
using Masaafa.WebApi.Models.Items;

namespace Masaafa.WebApi.Validators.Items;

public class CreateItemGroupRequestValidator : AbstractValidator<CreateItemGroupRequest>
{
    public CreateItemGroupRequestValidator()
    {
        RuleFor(entity => entity.Name).NotNull().NotEmpty().WithMessage("WarehouseItem's name cant be null or empty.");

        RuleFor(entity => entity.Description).NotNull().NotEmpty().WithMessage("WarehouseItem's description cant be null or empty.");
    }
}