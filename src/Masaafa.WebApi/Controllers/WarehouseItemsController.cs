using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Filters;
using Masaafa.WebApi.Models.Warehouses;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

[CustomAuthorize(nameof(UserRole.WarehouseOperator), nameof(UserRole.Supervisor), nameof(UserRole.SalesDirector))]
public class WarehouseItemsController(
    IMapper mapper,
    IValidator<CreateWarehouseItemRequest> createValidator,
    IValidator<UpdateWarehouseItemRequest> updateValidator,
    IWarehouseItemService service,
    IHeaderService headerService
    ) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search = null)
    {
        var result = await service.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<WarehouseItemResponse>>(result.Data));
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await service.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<WarehouseItemResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateWarehouseItemRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await service.CreateAsync(mapper.Map<WarehouseItem>(request), CancellationToken);

        return Ok(mapper.Map<WarehouseItemResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateWarehouseItemRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await service.UpdateAsync(id, mapper.Map<WarehouseItem>(request), CancellationToken);

        return Ok(mapper.Map<WarehouseItemResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await service.DeleteAsync(id, CancellationToken);

        return Ok(result);
    }
}