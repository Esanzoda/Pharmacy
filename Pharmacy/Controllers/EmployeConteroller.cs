using Microsoft.AspNetCore.Mvc;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;
using Pharmasy.Services;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeConteroller : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeConteroller(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<ActionResult<EmployeResponse>> CreateEmployee([FromBody] EmployeRequest request)
    {
        var response = await _employeeService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<EmployeResponse>> UpdateEmployee(long id, [FromBody] EmployeRequest request)
    {
        var response = await _employeeService.UpdateAsync(id, request);
        return Ok(response);
    }

    [HttpGet("id")]
    public async Task<ActionResult<ActionResult<EmployeResponse>>> GetEmployeeById(long id)
    {
        var response = await _employeeService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetAllEmployeesByPagenation(int pageNumber, int pageSize)
    {
        var response = await _employeeService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeById(long id)
    {
        var response = await _employeeService.DeleteAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetEmployeesByNameAsync(string name, int page, int pageSize)
    {
        var response = await _employeeService.GetEmployeesByNameAsync(name, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetEmployeesByAdressAsync(string adress, int page,
        int pageSize)
    {
        var response = await _employeeService.GetEmployeesByAdressAsync(adress, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetEmployeesByNumberAsync(string number)
    {
        var response = await _employeeService.GetEmployeesByNumberAsync(number);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<EmployeResponse>> GetEmployeeByEmailAsync(string email)
    {
        var response = await _employeeService.GetEmployeeByEmailAsync(email);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetEmployeesBySalaryAsync(decimal salary, int page,
        int pageSize)
    {
        var response = await _employeeService.GetEmployeesBySalaryAsync(salary, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetAllEmployeeByRoleAsync(Role role, int page, int pageSize)
    {
        var response = await _employeeService.GetAllEmployeeByRoleAsync(role, page, pageSize);

        return Ok(response);
    }
}