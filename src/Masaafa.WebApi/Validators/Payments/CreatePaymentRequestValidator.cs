using FluentValidation;
using Masaafa.WebApi.Models.Payments;

namespace Masaafa.WebApi.Validators.Payments;

public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentRequestValidator()
    {
        RuleFor(entity => entity.PaymentNumber)
            .NotNull().NotEmpty().WithMessage("Payment number cant be null or empty.");
    }
}