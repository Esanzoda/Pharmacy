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
        try
        {
            var response = await _customerService.CreateAsync(request);
            return Ok(response);
        }catch (ResourseIsAlredyExsistExeption ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<CustomerResponse>> UpdateCustomer(long id, [FromBody] CustomerRequest request)
    {
        try
        {
            var response = await _customerService.UpdateAsync(id, request);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("id")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerById(long id)
    {
        try
        {
            var response = await _customerService.GetByIdAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAllCustomersByPagenation(int pageNumber, int pageSize)
    {
        try
        {
            var response = await _customerService.GetAllByPaginationAsync(pageNumber, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCustomerById(long id)
    {
        try
        {
            var response = await _customerService.DeleteAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse?>> GetCustomerByEmailAsync(string email)
    {
        try
        {
            var response = await _customerService.GetCustomerByEmailAsync(email);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse?>> GetCustomerByPhoneAsync(string phone)
    {
        try
        {
            var response = await _customerService.GetCustomerByPhoneAsync(phone);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetCustomerByNameAsync(string name)
    {
        try
        {
            var response = await _customerService.GetCustomerByNameAsync(name);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }
}