using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmasy.CQRS.Auth.Commands;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Register([FromBody] RegisterCommand registerCommandHandler)
    {
        var response = await _mediator.Send(registerCommandHandler);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> ReCreateToken(ReGenerateRefreshTokenCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [Authorize]
    [HttpPatch]
    public async Task<ActionResult<string>> ChangePassword(string oldPassword, string newPassword)
    {
        return Ok("changed");
    }
}