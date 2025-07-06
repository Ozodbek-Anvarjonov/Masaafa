using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Models.Payments;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

public class PaymentsController(
    IMapper mapper,
    IHeaderService headerService,
    IPaymentService paymentService,
    IValidator<CreatePaymentRequest> createValidator,
    IValidator<UpdatePaymentRequest> updateValidator
    ) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search
        )
    {
        var result = await paymentService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<PaymentResponse>>(result.Data));
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await paymentService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<PaymentResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreatePaymentRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await paymentService.CreateAsync(mapper.Map<Payment>(request), CancellationToken);

        return Ok(mapper.Map<PaymentResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdatePaymentRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await paymentService.UpdateAsync(id, mapper.Map<Payment>(request), CancellationToken);

        return Ok(mapper.Map<PaymentResponse>(entity));
    }

    [HttpPatch("{id:guid}")]
    public async ValueTask<IActionResult> Patch([FromRoute] Guid id, [FromBody] UpdatePaymentCompleteRequest request)
    {
        var entity = await paymentService.UpdateCompleteAsync(id, mapper.Map<Payment>(request), CancellationToken);

        return Ok(mapper.Map<PaymentResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await paymentService.DeleteByIdAsync(id, CancellationToken);

        return Ok(result);
    }
}