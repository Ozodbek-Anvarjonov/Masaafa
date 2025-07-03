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
    IWarehouseService service,
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
        var result = await service.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<WarehouseResponse>>(result.Data));
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await service.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<WarehouseResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateWarehouseRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await service.CreateAsync(mapper.Map<Warehouse>(request), CancellationToken);

        return Ok(mapper.Map<WarehouseResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateWarehouseRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await service.UpdateAsync(id, mapper.Map<Warehouse>(request), CancellationToken);

        return Ok(mapper.Map<WarehouseResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await service.DeleteAsync(id, CancellationToken);

        return Ok(result);
    }
}