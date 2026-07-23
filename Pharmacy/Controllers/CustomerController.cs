using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Customer.Commands;
using Pharmacy.CQRS.Customer.Queries;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CustomerController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create([FromBody] CustomerRequest request)
    {
        var response = await mediator.Send(new CreateCustomerCommand(request));
        return Ok(response);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<CustomerResponse>> Update([FromBody] UpdateCustomerRequest request)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateCustomerCommand(customerId, request));
        return Ok(response);
    }

    [Authorize]
    [HttpPatch]
    public async Task<ActionResult<CustomerResponse>> UpdatePassword([FromBody] string newPassword)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateCustomerPasswordCommand(customerId, newPassword));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetById(long id)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new GetCustomerByIdQuery(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAllByPagination(int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllCustomerByPaginationQuery(pageNumber, pageSize),
            HttpContext.RequestAborted);
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await mediator.Send(new DeleteCustomerCommand(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CustomerResponse?>> GetByEmailAsync(string email)
    {
        var response = await mediator.Send(new GetCustomerByEmailQuery(email));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CustomerResponse?>> GetByPhoneAsync(string phone)
    {
        var response = await mediator.Send(new GetCustomerByPhoneNumberQuery(phone));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetByNameAsync(string name)
    {
        var response = await mediator.Send(new GetCustomerByNameQuery(name));
        return Ok(response);
    }
}