using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Pharmacy.CQRS.Cart.Commands;
using Pharmacy.Models.Dto.Request;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = "Customer")]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController( IMediator mediator)
    {
        
        _mediator = mediator;
    }

   [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateItemQuantity(long productId,int quantity)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _mediator.Send(new UpdateQuantityCartItemCommand(customerId, productId, quantity));
        return Ok(response);
    }
   [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToCartAsync(CartItemRequest request)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _mediator.Send(new AddItemToCartCommand(customerId,request));
        return Ok(response);
    }
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> RemoveItemFromCartAsync(long itemId)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _mediator.Send(new DeleteItemFromCartCommand(customerId, itemId));
        return Ok(response);
    }
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> ClearCartAsync()
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _mediator.Send(new ClearCartCommand(customerId));
        return Ok(response);
    }
    [Authorize]
    [HttpPatch]
    public async Task<IActionResult> UpdateQuantityItemAsync(long cartItemId, int quantity)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _mediator.Send(new UpdateQuantityCartItemCommand(customerId, cartItemId, quantity));
        return Ok(response);
    }
}