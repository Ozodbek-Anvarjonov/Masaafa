using FluentValidation;
using Masaafa.WebApi.Models.TransferRequests;

namespace Masaafa.WebApi.Validators.TransferRequests;

public class CreateTransferItemRequestValidator : AbstractValidator<CreateTransferItemRequest>
{
    public CreateTransferItemRequestValidator()
    {
        RuleFor(entity => entity.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cant be lower than 0.");

        RuleFor(entity => entity.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Unit price cant be lower than 0.");

        RuleFor(entity => entity.SentQuantity)
            .Must(entity => !entity.HasValue || entity.Value >= 0).WithMessage("Sent quantity cant be lower than 0.");

        RuleFor(entity => entity.ReceivedQuantity)
            .Must(entity => !entity.HasValue || entity.Value >= 0).WithMessage("Received quantity cant be lower than 0.");
    }
}