using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Filters;
using Masaafa.WebApi.Models.TransferRequests;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

[CustomAuthorize(nameof(UserRole.WarehouseOperator), nameof(UserRole.Supervisor), nameof(UserRole.SalesDirector))]
public class TransferItemsController(
    IMapper mapper,
    IHeaderService headerService,
    ITransferRequestItemService transferRequestItemService,
    IValidator<CreateTransferItemRequest> createValidator,
    IValidator<UpdateTransferItemRequest> updateValidator
    ) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search
        )
    {
        var result = await transferRequestItemService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<TransferItemResponse>>(result.Data));
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await transferRequestItemService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<TransferItemResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateTransferItemRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await transferRequestItemService.CreateAsync(mapper.Map<TransferRequestItem>(request), CancellationToken);

        return Ok(mapper.Map<TransferItemResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateTransferItemRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await transferRequestItemService.UpdateAsync(id, mapper.Map<TransferRequestItem>(request), CancellationToken);

        return Ok(mapper.Map<TransferItemResponse>(entity));
    }

    [HttpPatch("{transferItemId:guid}/{fromWarehouseItemId}/{toWarehouseItemId}")]
    public async ValueTask<IActionResult> PatchWarehouse([FromRoute] Guid transferItemId, [FromRoute] Guid fromWarehouseItemId, [FromRoute] Guid toWarehouseItemId)
    {
        var entity = await transferRequestItemService.UpdateAsync(transferItemId,
            new TransferRequestItem() { Id = transferItemId, FromWarehouseItemId = fromWarehouseItemId, ToWarehouseItemId = toWarehouseItemId },
            CancellationToken);

        return Ok(mapper.Map<TransferItemResponse>(entity));
    }

    [HttpPatch("{id:guid}/send-date")]
    public async ValueTask<IActionResult> PatchSentDate([FromRoute] Guid id, [FromBody] UpdateTransferItemSentDate request)
    {
        var entity = await transferRequestItemService.UpdateSendDateAsync(id, mapper.Map<TransferRequestItem>(request), CancellationToken);

        return Ok(mapper.Map<TransferItemResponse>(entity));
    }

    [HttpPatch("{id:guid}/receive-date")]
    public async ValueTask<IActionResult> PatchReceiveDate([FromRoute] Guid id, [FromBody] UpdateTransferItemReceiveDate request)
    {
        var entity = await transferRequestItemService.UpdateReceiveAsync(id,
            mapper.Map<TransferRequestItem>(request),
            CancellationToken);

        return Ok(mapper.Map<TransferItemResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await transferRequestItemService.DeleteByIdAsync(id, CancellationToken);

        return Ok(result);
    }
}