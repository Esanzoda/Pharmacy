using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using MediatR;
using Pharmasy.Services;
using Pharmasy.Services.Employee.Command;
using Pharmasy.Services.Employee.Query;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeConteroller : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMediator _mediator;

    public EmployeConteroller(IEmployeeService employeeService, IMediator mediator)
    {
        _employeeService = employeeService;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeResponse>> Create([FromBody] EmployeeRequest request)
    {
        var response = await _mediator.Send(new CreateEmployeCommand(request));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<EmployeeResponse>> Update(long id, [FromBody] EmployeeRequest request)
    {
        var response = await _mediator.Send(new UpdateEmployeCommand(id, request));
        return Ok(response);
    }

    [HttpGet("id")]
    public async Task<ActionResult<ActionResult<EmployeeResponse>>> GetById(long id)
    {
        var response = await _employeeService.GetByIdAsync(id, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAllByPagenation(int pageNumber, int pageSize)
    {
        var response = await _employeeService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await _mediator.Send(new DeleteEmployeCommand(id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByNameAsync(string name, int page, int pageSize)
    {
        var response = await _mediator.Send(new GetEmpoyeesByNameQuery(name, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByAdressAsync(string adress, int page,
        int pageSize)
    {
        var response = await _mediator.Send(new GetEmpoyeesByAddressQuery(adress, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByNumberAsync(string number)
    {
        var response = await _mediator.Send(new GetEmpoyeesByNumberQuery(number));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<EmployeeResponse>> GetByEmailAsync(string email)
    {
        var response = await _mediator.Send(new GetEmployeeByEmailQuery(email));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetBySalaryAsync(decimal salary, int page,
        int pageSize)
    {
        var response = await _mediator.Send(new GetEmployeeBySalaryQuery(salary, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByRoleAsync(Role role, int page, int pageSize)
    {
        var response = await _mediator.Send(new GetEmployeeByRoleQuery(role, page, pageSize));
        return Ok(response);
    }
}