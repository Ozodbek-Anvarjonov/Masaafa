using FluentValidation;
using Masaafa.WebApi.Models.SalesOrders;

namespace Masaafa.WebApi.Validators.SalesOrders;

public class CreateSalesOrderItemRequestValidator : AbstractValidator<CreateSalesOrderItemRequest>
{
    public CreateSalesOrderItemRequestValidator()
    {
        RuleFor(entity => entity.DiscountPercent)
            .GreaterThanOrEqualTo(0).WithMessage("Discount percent cant be lower than 0.");

        RuleFor(entity => entity.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cant be lower than 0.");

        RuleFor(entity => entity.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Unit price cant be lower than 0.");
    }
}