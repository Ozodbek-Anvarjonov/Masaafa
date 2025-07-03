using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Models.Warehouses;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

public class WarehousesController(
    IMapper mapper,
    IValidator<CreateWarehouseRequest> createValidator,
    IValidator<UpdateWarehouseRequest> updateValidator,
    IWarehouseService warehouseService,
    IWarehouseItemService warehouseItemService,
    IHeaderService headerService
    ) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search = null
        )
    {
        var result = await warehouseService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<WarehouseResponse>>(result.Data));
    }

    [HttpGet("{warehouseId:guid}/warehouse-items")]
    public async ValueTask<IActionResult> GetWarehouseItemsById(
        [FromRoute] Guid warehouseId,
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search = null
        )
    {
        var result = await warehouseItemService.GetByWarehouseIdAsync(warehouseId, @params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<WarehouseItemResponse>>(result.Data));
    }

    [HttpGet("{warehouseId:guid}/warehouse-items/{itemId:guid}")]
    public async ValueTask<IActionResult> GetWarehouseItemById([FromRoute] Guid warehouseId, [FromRoute] Guid itemId)
    {
        var entity = await warehouseItemService.GetByWarehouseIdAndItemIdAsync(warehouseId, itemId, CancellationToken);

        return Ok(mapper.Map<WarehouseItemResponse>(entity));
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await warehouseService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<WarehouseResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateWarehouseRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await warehouseService.CreateAsync(mapper.Map<Warehouse>(request), CancellationToken);

        return Ok(mapper.Map<WarehouseResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateWarehouseRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await warehouseService.UpdateAsync(id, mapper.Map<Warehouse>(request), CancellationToken);

        return Ok(mapper.Map<WarehouseResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await warehouseService.DeleteAsync(id, CancellationToken);

        return Ok(result);
    }
}