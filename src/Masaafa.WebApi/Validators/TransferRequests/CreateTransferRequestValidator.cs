using FluentValidation;
using Masaafa.WebApi.Models.TransferRequests;

namespace Masaafa.WebApi.Validators.TransferRequests;

public class CreateTransferRequestValidator : AbstractValidator<CreateTransferRequest>
{
    public CreateTransferRequestValidator()
    {
        RuleFor(entity => entity.RequestNumber)
            .NotNull().NotEmpty().WithMessage("Request number cant be null or empty.");
    }
}