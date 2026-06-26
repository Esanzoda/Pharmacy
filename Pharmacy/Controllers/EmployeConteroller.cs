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
        try
        {
            var response = await _employeeService.CreateAsync(request);
            return Ok(response);
        }
        catch (ResourseIsAlredyExsistExeption ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
            
        }
    }

    [HttpPut]
    public async Task<ActionResult<EmployeResponse>> UpdateEmployee(long id, [FromBody] EmployeRequest request)
    {
        try
        {
            var response = await _employeeService.UpdateAsync(id, request);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("id")]
    public async Task<ActionResult<ActionResult<EmployeResponse>>> GetEmployeeById(long id)
    {
        try
        {
            var response = await _employeeService.GetByIdAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetAllEmployeesByPagenation(int pageNumber, int pageSize)
    {
        try
        {
            var response = await _employeeService.GetAllByPaginationAsync(pageNumber, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<IActionResult>> DeleteEmployeeById(long id)
    {
        try
        {
            var response = await _employeeService.DeleteAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetEmployeesByNameAsync(string name, int page, int pageSize)
    {
        try
        {
            var response = await _employeeService.GetEmployeesByNameAsync(name, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetEmployeesByAdressAsync(string adress, int page,
        int pageSize)
    {
        try
        {
            var response = await _employeeService.GetEmployeesByAdressAsync(adress, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetEmployeesByNumberAsync(string number, int page,
        int pageSize)
    {
        try
        {
            var response = await _employeeService.GetEmployeesByNumberAsync(number, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<EmployeResponse>> GetEmployeeByEmailAsync(string email)
    {
        try
        {
            var response = await _employeeService.GetEmployeeByEmailAsync(email);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetEmployeesBySalaryAsync(decimal salary, int page,
        int pageSize)
    {
        try
        {
            var response = await _employeeService.GetEmployeesBySalaryAsync(salary, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetAllEmployeeByRoleAsync(Role role, int page, int pageSize)
    {
        try
        {
            var response = await _employeeService.GetAllEmployeeByRoleAsync(role, page, pageSize);

            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }
}