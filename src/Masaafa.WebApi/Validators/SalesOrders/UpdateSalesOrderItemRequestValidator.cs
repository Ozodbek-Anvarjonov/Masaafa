using FluentValidation;
using Masaafa.WebApi.Models.SalesOrders;

namespace Masaafa.WebApi.Validators.SalesOrders;

public class UpdateSalesOrderItemRequestValidator : AbstractValidator<UpdateSalesOrderItemRequest>
{
    public UpdateSalesOrderItemRequestValidator()
    {
        RuleFor(entity => entity.DiscountPercent)
            .GreaterThanOrEqualTo(0).WithMessage("Discount percent cant be lower than 0.");

        RuleFor(entity => entity.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cant be lower than 0.");

        RuleFor(entity => entity.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Unit price cant be lower than 0.");
    }
}