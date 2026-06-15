using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]

public class CustomerController:ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> CreateCategory([FromBody] CustomerRequest request)
    {
        var response =await  _customerService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CustomerResponse>> UpdateCategory([FromBody] long id, CustomerRequest request)
    {
        var response =await _customerService.UpdateAsync(id,request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetCategoryById(long id)
    {
        var response =await _customerService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAllCategoriesByPagenation(int pageNumber, int pageSize)
    {
        var response=_customerService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategoryById(long id)
    {
        var response =await _customerService.DeleteAsync(id);
        return Ok(response);
    }
    
}