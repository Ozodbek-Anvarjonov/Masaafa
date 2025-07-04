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

public class InventoriesController(
    IMapper mapper,
    IHeaderService headerService,
    IInventoryService inventoryService,
    IValidator<CreateInventoryRequest> createValidator,
    IValidator<UpdateInventoryRequest> updateValidator
    ) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
    [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search
        )
    {
        var result = await inventoryService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<InventoryResponse>>(result.Data));
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await inventoryService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<InventoryResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateInventoryRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await inventoryService.CreateAsync(mapper.Map<Inventory>(request), CancellationToken);

        return Ok(mapper.Map<InventoryResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateInventoryRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await inventoryService.UpdateAsync(id, mapper.Map<Inventory>(request), CancellationToken);
        return Ok(mapper.Map<InventoryResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await inventoryService.DeleteByIdAsync(id, CancellationToken);

        return Ok(result);
    }
}