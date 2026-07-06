using Microsoft.AspNetCore.Mvc;
using Pharmasy.Exeption;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromBody] CustomerRequest request)
    {
        var response = await _customerService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CustomerResponse>> UpdateCustomer(long id, [FromBody] CustomerRequest request)
    {
        var response = await _customerService.UpdateAsync(id, request);
        return Ok(response);
    }

    [HttpGet("id")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerById(long id)
    {
        var response = await _customerService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAllCustomersByPagenation(int pageNumber, int pageSize)
    {
        var response = await _customerService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCustomerById(long id)
    {
        var response = await _customerService.DeleteAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse?>> GetCustomerByEmailAsync(string email)
    {
        var response = await _customerService.GetCustomerByEmailAsync(email);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse?>> GetCustomerByPhoneAsync(string phone)
    {
        var response = await _customerService.GetCustomerByPhoneAsync(phone);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetCustomerByNameAsync(string name)
    {
        var response = await _customerService.GetCustomerByNameAsync(name);
        return Ok(response);
    }
}