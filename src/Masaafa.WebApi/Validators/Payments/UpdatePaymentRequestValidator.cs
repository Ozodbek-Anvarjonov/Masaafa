using FluentValidation;
using Masaafa.WebApi.Models.Payments;

namespace Masaafa.WebApi.Validators.Payments;

public class UpdatePaymentRequestValidator : AbstractValidator<UpdatePaymentRequest>
{
    public UpdatePaymentRequestValidator()
    {
        RuleFor(entity => entity.PaymentNumber)
            .NotNull().NotEmpty().WithMessage("Payment number cant be null or empty.");
    }
}