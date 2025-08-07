using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.Infrastructure.Services;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Filters;
using Masaafa.WebApi.Models.SalesOrders;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

[CustomAuthorize(nameof(UserRole.Agent), nameof(UserRole.WarehouseOperator), nameof(UserRole.Supervisor), nameof(UserRole.SalesDirector))]
public class SalesOrderItemsController(
    IMapper mapper,
    IHeaderService headerService,
    ISalesOrderItemService salesOrderItemService,
    IValidator<CreateSalesOrderItemRequest> createValidator,
    IValidator<UpdateSalesOrderItemRequest> updateValidator
    ) : BaseController
{
    [CustomAuthorize(nameof(UserRole.Client), nameof(UserRole.Agent), nameof(UserRole.WarehouseOperator), nameof(UserRole.Supervisor), nameof(UserRole.SalesDirector))]
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search
        )
    {
        var result = await salesOrderItemService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<SalesOrderItemResponse>>(result.Data));
    }

    [CustomAuthorize(nameof(UserRole.Client), nameof(UserRole.Agent), nameof(UserRole.WarehouseOperator), nameof(UserRole.Supervisor), nameof(UserRole.SalesDirector))]
    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await salesOrderItemService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<SalesOrderItemResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateSalesOrderItemRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await salesOrderItemService.CreateAsync(mapper.Map<SalesOrderItem>(request), CancellationToken);

        return Ok(mapper.Map<SalesOrderItemResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateSalesOrderItemRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await salesOrderItemService.UpdateAsync(id, mapper.Map<SalesOrderItem>(request), CancellationToken);

        return Ok(mapper.Map<SalesOrderItemResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await salesOrderItemService.DeleteByIdAsync(id, CancellationToken);

        return Ok(result);
    }


    [HttpPatch("{salesOrderId:guid}/{warehouseId:guid}")]
    public async ValueTask<IActionResult> PatchWarehouse([FromRoute] Guid salesOrderId, [FromRoute] Guid warehouseId)
    {
        var result = await salesOrderItemService.UpdateWarehouseAsync(salesOrderId, warehouseId, CancellationToken);

        return Ok(mapper.Map<SalesOrderResponse>(result));
    }

    [HttpPatch("{id:guid}/send")]
    public async ValueTask<IActionResult> PatchSendDate([FromRoute] Guid id, [FromBody] UpdateSalesOrderItemSendDate request)
    {
        var entity = await salesOrderItemService.UpdateSendAsync(id, mapper.Map<SalesOrderItem>(request), CancellationToken);

        return Ok(mapper.Map<SalesOrderItemResponse>(entity));
    }

    [HttpPatch("{id:guid}/receive")]
    public async ValueTask<IActionResult> PatchReceiveDate([FromRoute] Guid id, [FromBody] UpdateSalesOrderItemReceiveDate request)
    {
        var entity = await salesOrderItemService.UpdateReceiveAsync(id, mapper.Map<SalesOrderItem>(request), CancellationToken);

        return Ok(mapper.Map<SalesOrderItemResponse>(entity));
    }
}