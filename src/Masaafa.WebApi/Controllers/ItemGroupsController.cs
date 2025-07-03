using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Models.Items;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

public class ItemGroupsController(
    IMapper mapper,
    IValidator<CreateItemGroupRequest> createValidator,
    IValidator<UpdateItemGroupRequest> updateValidator,
    IItemGroupService service,
    IHeaderService headerService) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search = null)
    {
        var result = await service.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<ItemGroupResponse>>(result.Data));
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await service.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<ItemGroupResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateItemGroupRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await service.CreateAsync(mapper.Map<ItemGroup>(request), CancellationToken);

        return Ok(mapper.Map<ItemGroupResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateItemGroupRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await service.UpdateAsync(id, mapper.Map<ItemGroup>(request), CancellationToken);

        return Ok(mapper.Map<ItemGroupResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await service.DeleteAsync(id, CancellationToken);

        return Ok(result);
    }
}