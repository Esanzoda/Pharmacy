using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;
using Pharmasy.Services.Customer.Command;
using Pharmasy.Services.Customer.Query;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create([FromBody] CustomerRequest request)
    {
        var response = await _mediator.Send(new CreateCustomerCommand(request));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CustomerResponse>> Update(long id, [FromBody] UpdateCustomerRequest request)
    {
        var response = await _mediator.Send(new UpdateCustomerCommand(id, request));
        return Ok(response);
    }
    [HttpPatch]
    public async Task<ActionResult<CustomerResponse>> UpdatePassword(long id, [FromBody] string newPassword)
    {
        var response = await _mediator.Send(new UpdateCustomerPasswordCommand(id, newPassword));
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> GetById(long id)
    {
        var response = await _mediator.Send(new GetCustomerByIdQuery(id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAllByPagenation(int pageNumber, int pageSize)
    {
        var response = await _mediator.Send(new GetAllCustomerByPagenationQuery(pageNumber, pageSize));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response =await _mediator.Send(new DeleteCustomerCommand(id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse?>> GetByEmailAsync(string email)
    {
        var response = await _mediator.Send(new GetCustomerByEmailQuery(email));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse?>> GetByPhoneAsync(string phone)
    {
        var response = await _mediator.Send(new GetCustomerByPhoneNumberQuery(phone));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetByNameAsync(string name)
    {
        var response = await _mediator.Send(new GetCustomerByNameQuery(name));
        return Ok(response);
    }
}