using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Pharmasy.CQRS.Employee.Commands;
using Pharmasy.CQRS.Employee.Queries;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeConteroller(IMediator mediator) : ControllerBase
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
    public async Task<ActionResult<List<EmployeeResponse>>> GetAllByPagenation(int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllEmployeeByPagenationQuery(pageNumber, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await mediator.Send(new DeleteEmployeCommand(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByNameAsync(string name, int page, int pageSize)
    {
        var response = await mediator.Send(new GetEmpoyeesByNameQuery(name, page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByAdressAsync(string adress, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetEmpoyeesByAddressQuery(adress, page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByNumberAsync(string number)
    {
        var response = await mediator.Send(new GetEmpoyeesByNumberQuery(number));
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