using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;
using Pharmasy.Services;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeConteroller:ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeConteroller(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    
    [HttpPost]
    public async Task<ActionResult<EmployeResponse>> CreateCategory([FromBody] EmployeRequest request)
    {
        var response =await  _employeeService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<EmployeResponse>> UpdateCategory([FromBody] long id, EmployeRequest request)
    {
        var response =await _employeeService.UpdateAsync(id,request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<EmployeResponse>> GetCategoryById(long id)
    {
        var response =await _employeeService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeResponse>>> GetAllCategoriesByPagenation(int pageNumber, int pageSize)
    {
        var response=_employeeService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategoryById(long id)
    {
        var response =await _employeeService.DeleteAsync(id);
        return Ok(response);
    }
}