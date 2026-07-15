using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services.Cart.AuthService.Command;

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
    public async Task<ActionResult<CustomerResponse>> Register([FromBody] RegisterCommand registerCommand)
    {
        var response = await _mediator.Send(registerCommand);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> ReCreateToken(ReGenerateRefreshTokenComman request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> ChangePassword(string oldPassword, string newPassword)
    {
        return Ok("changed");
    }
}