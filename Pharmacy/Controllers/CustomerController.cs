using AutoMapper;
using MassTransit.Internals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;
using Pharmasy.Services.Customer.Command;
using Pharmasy.Services.Customer.Query;
using static Pharmasy.Models.Domain.Enum.Role;

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

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create([FromBody] CustomerRequest request)
    {
        var response = await _mediator.Send(new CreateCustomerCommand(request));
        return Ok(response);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<CustomerResponse>> Update(long id, [FromBody] UpdateCustomerRequest request)
    {
        var response = await _mediator.Send(new UpdateCustomerCommand(id, request));
        return Ok(response);
    }

    [Authorize]
    [HttpPatch]
    public async Task<ActionResult<CustomerResponse>> UpdatePassword(long id, [FromBody] string newPassword)
    {
        var response = await _mediator.Send(new UpdateCustomerPasswordCommand(id, newPassword));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetById(long id)
    {
        var response = await _mediator.Send(new GetCustomerByIdQuery(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAllByPagenation(int pageNumber, int pageSize)
    {
        var response = await _mediator.Send(new GetAllCustomerByPagenationQuery(pageNumber, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await _mediator.Send(new DeleteCustomerCommand(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CustomerResponse?>> GetByEmailAsync(string email)
    {
        var response = await _mediator.Send(new GetCustomerByEmailQuery(email));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CustomerResponse?>> GetByPhoneAsync(string phone)
    {
        var response = await _mediator.Send(new GetCustomerByPhoneNumberQuery(phone));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetByNameAsync(string name)
    {
        var response = await _mediator.Send(new GetCustomerByNameQuery(name));
        return Ok(response);
    }
}