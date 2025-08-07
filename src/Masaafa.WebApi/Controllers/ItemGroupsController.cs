using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Filters;
using Masaafa.WebApi.Models.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

[CustomAuthorize(nameof(UserRole.WarehouseOperator), nameof(UserRole.Supervisor), nameof(UserRole.SalesDirector))]
public class ItemGroupsController(
    IMapper mapper,
    IValidator<CreateItemGroupRequest> createValidator,
    IValidator<UpdateItemGroupRequest> updateValidator,
    IItemGroupService itemGroupService,
    IItemService itemService,
    IHeaderService headerService) : BaseController
{
    [CustomAuthorize(nameof(UserRole.Client), nameof(UserRole.Agent), nameof(UserRole.WarehouseOperator), nameof(UserRole.Supervisor), nameof(UserRole.SalesDirector))]
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search = null)
    {
        var result = await itemGroupService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<ItemGroupResponse>>(result.Data));
    }

    [AllowAnonymous]
    [HttpGet("{itemGroupId:guid}/items")]
    public async ValueTask<IActionResult> GetItemsById(
        [FromRoute] Guid itemGroupId,
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search = null)
    {
        var result = await itemService.GetByGroupIdAsync(itemGroupId, @params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<ItemResponse>>(result.Data));
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await itemGroupService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<ItemGroupResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateItemGroupRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await itemGroupService.CreateAsync(mapper.Map<ItemGroup>(request), CancellationToken);

        return Ok(mapper.Map<ItemGroupResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateItemGroupRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await itemGroupService.UpdateAsync(id, mapper.Map<ItemGroup>(request), CancellationToken);

        return Ok(mapper.Map<ItemGroupResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await itemGroupService.DeleteAsync(id, CancellationToken);

        return Ok(result);
    }
}