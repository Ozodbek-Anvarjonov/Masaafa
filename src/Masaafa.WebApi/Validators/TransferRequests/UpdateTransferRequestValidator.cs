using FluentValidation;
using Masaafa.WebApi.Models.TransferRequests;

namespace Masaafa.WebApi.Validators.TransferRequests;

public class UpdateTransferRequestValidator : AbstractValidator<UpdateTransferRequest>
{
    public UpdateTransferRequestValidator()
    {
        RuleFor(entity => entity.RequestNumber)
            .NotNull().NotEmpty().WithMessage("Request number cant be null or empty.");
    }
}