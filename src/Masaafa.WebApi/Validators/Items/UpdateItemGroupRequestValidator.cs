using FluentValidation;
using Masaafa.WebApi.Models.Items;

namespace Masaafa.WebApi.Validators.Items;

public class UpdateItemGroupRequestValidator : AbstractValidator<UpdateItemGroupRequest>
{
    public UpdateItemGroupRequestValidator()
    {
        RuleFor(entity => entity.Name).NotNull().NotEmpty().WithMessage("WarehouseItem's name cant be null or empty.");

        RuleFor(entity => entity.Description).NotNull().NotEmpty().WithMessage("WarehouseItem's description cant be null or empty.");
    }
}