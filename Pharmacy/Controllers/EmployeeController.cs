using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Employee.Commands;
using Pharmacy.CQRS.Employee.Queries;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeeController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<EmployeeResponse>> Create([FromBody] EmployeeRequest request)
    {
        var response = await mediator.Send(new CreateEmployeeCommand(request));
        return Ok(response);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<EmployeeResponse>> Update([FromBody] EmployeeRequest request)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateEmployeeCommand(employeeId, request));
        return Ok(response);
    }

    [Authorize]
    [HttpGet("id")]
    public async Task<ActionResult<ActionResult<EmployeeResponse>>> GetById(long id)
    {
        var response = await mediator.Send(new GetEmployeeByIdQuery(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAllByPagination(int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllEmployeeByPaginationQuery(pageNumber, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await mediator.Send(new DeleteEmployeeCommand(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByNameAsync(string name, int page, int pageSize)
    {
        var response = await mediator.Send(new GetEmployeesByNameQuery(name, page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByAddressAsync(string address, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetEmployeesByAddressQuery(address, page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByNumberAsync(string number)
    {
        var response = await mediator.Send(new GetEmployeesByNumberQuery(number));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<EmployeeResponse>> GetByEmailAsync(string email)
    {
        var response = await mediator.Send(new GetEmployeeByEmailQuery(email));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetBySalaryAsync(decimal salary, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetEmployeeBySalaryQuery(salary, page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByRoleAsync(Role role, int page, int pageSize)
    {
        var response = await mediator.Send(new GetEmployeeByRoleQuery(role, page, pageSize));
        return Ok(response);
    }
}