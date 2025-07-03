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

public class ItemsController(
    IMapper mapper,
    IValidator<CreateItemRequest> createValidator,
    IValidator<UpdateItemRequest> updateValidator,
    IItemService itemService,
    IHeaderService headerService
    ) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search = null)
    {
        var result = await itemService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<ItemResponse>>(result.Data));
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await itemService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<ItemResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateItemRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await itemService.CreateAsync(mapper.Map<Item>(request), CancellationToken);

        return Ok(mapper.Map<ItemResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateItemRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await itemService.UpdateAsync(id, mapper.Map<Item>(request), CancellationToken);

        return Ok(mapper.Map<ItemResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await itemService.DeleteAsync(id, CancellationToken);

        return Ok(result);
    }
}