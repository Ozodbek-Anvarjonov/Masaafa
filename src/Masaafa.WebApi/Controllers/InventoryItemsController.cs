using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Models.Inventories;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

public class InventoryItemsController(
    IMapper mapper,
    IHeaderService headerService,
    IInventoryItemService inventoryItemService,
    IValidator<CreateInventoryItemRequest> createValidator,
    IValidator<UpdateInventoryItemRequest> updateValidator
    ) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search
        )
    {
        var result = await inventoryItemService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<InventoryItemResponse>>(result.Data));
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await inventoryItemService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<InventoryItemResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateInventoryItemRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await inventoryItemService.CreateAsync(mapper.Map<InventoryItem>(request), CancellationToken);

        return Ok(mapper.Map<InventoryItemResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateInventoryItemRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await inventoryItemService.UpdateAsync(id, mapper.Map<InventoryItem>(request), CancellationToken);

        return Ok(mapper.Map<InventoryItemResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await inventoryItemService.DeleteByIdAsync(id, CancellationToken);

        return Ok(result);
    }
}