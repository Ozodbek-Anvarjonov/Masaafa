using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Models.TransferRequests;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

public class TransfersController(
    IMapper mapper,
    IHeaderService headerService,
    ITransferRequestService transferRequestService,
    IValidator<CreateTransferRequest> createValidator,
    IValidator<UpdateTransferRequest> updateValidator
    ) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search
        )
    {
        var result = await transferRequestService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<TransferResponse>>(result.Data));
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await transferRequestService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<TransferResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateTransferRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await transferRequestService.CreateAsync(mapper.Map<TransferRequest>(request), CancellationToken);

        return Ok(mapper.Map<TransferResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateTransferRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await transferRequestService.UpdateAsync(id, mapper.Map<TransferRequest>(request), CancellationToken);

        return Ok(mapper.Map<TransferResponse>(entity));
    }

    [HttpPatch("{transferId:guid}/{fromWarehouseId:guid}/{toWarehouseId:guid}")]
    public async ValueTask<IActionResult> PatchWarehouses(
        [FromRoute] Guid transferId,
        [FromRoute] Guid fromWarehouseId,
        [FromRoute] Guid toWarehouseId
        )
    {
        var entity = await transferRequestService.UpdateWarehousesAsync(transferId, 
            new() { FromWarehouseId = fromWarehouseId, ToWarehouseId = toWarehouseId },
            CancellationToken);

        return Ok(mapper.Map<TransferResponse>(entity));
    }

    [HttpPatch("{id:guid}/approve")]
    public async ValueTask<IActionResult> PatchApprove(
        [FromRoute] Guid id,
        [FromBody] UpdateTransferApproveRequest request
        )
    {
        var entity = await transferRequestService.UpdateApproveAsync(id,
            mapper.Map<TransferRequest>(request),
            CancellationToken);

        return Ok(mapper.Map<TransferResponse>(entity));
    }

    [HttpPatch("{id:guid}/reject")]
    public async ValueTask<IActionResult> PatchReject(
        [FromRoute] Guid id,
        [FromBody] UpdateTransferRejectRequest request
        )
    {
        var entity = await transferRequestService.UpdateRejectAsync(id,
            mapper.Map<TransferRequest>(request),
            CancellationToken);

        return Ok(mapper.Map<TransferResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await transferRequestService.DeleteByIdAsync(id, CancellationToken);

        return Ok(result);
    }
}