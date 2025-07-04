using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

public class EmployeesController(
    IEmployeeService employeeService,
    IHeaderService headerService,
    IMapper mapper,
    IValidator<CreateEmployeeRequest> createValidator,
    IValidator<UpdateEmployeeRequest> updateValidator) : BaseController

{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
    [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search = null)
    {
        var result = await employeeService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<EmployeeResponse>>(result.Data));
    }


    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await employeeService.GetByIdAsync(id, CancellationToken);
        return Ok(mapper.Map<EmployeeResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateEmployeeRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await employeeService.CreateAsync(mapper.Map<Employee>(request), CancellationToken);
        
        return Ok(mapper.Map<EmployeeResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateEmployeeRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await employeeService.UpdateAsync(id, mapper.Map<Employee>(request), CancellationToken);

        return Ok(mapper.Map<EmployeeResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await employeeService.DeleteByIdAsync(id, CancellationToken);

        return Ok(result);
    }
}