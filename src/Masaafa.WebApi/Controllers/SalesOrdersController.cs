using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Models.SalesOrders;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

public class SalesOrdersController(
    IMapper mapper,
    IHeaderService headerService,
    ISalesOrderService salesOrderService,
    IValidator<CreateSalesOrderRequest> createValidator,
    IValidator<UpdateSalesOrderRequest> updateValidator
    ) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search
        )
    {
        var result = await salesOrderService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<SalesOrderResponse>>(result.Data));
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await salesOrderService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<SalesOrderResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateSalesOrderRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await salesOrderService.CreateAsync(mapper.Map<SalesOrder>(request), CancellationToken);

        return Ok(mapper.Map<SalesOrderResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateSalesOrderRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await salesOrderService.UpdateAsync(id, mapper.Map<SalesOrder>(request), CancellationToken);

        return Ok(mapper.Map<SalesOrderResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await salesOrderService.DeleteByIdAsync(id, CancellationToken);

        return Ok(result);
    }
}