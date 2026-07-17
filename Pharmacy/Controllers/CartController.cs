using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Pharmasy.Services.Cart.Command;

namespace Pharmasy.Controllers;

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
    public async Task<IActionResult> UpdateItemQuantity(long customerId,long cartItemId,int quantity)
    {
        var response = await _mediator.Send(new UpdateQuantityCartItemCommand(customerId, cartItemId, quantity));
        return Ok(response);
    }
   [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToCartAsync(CartItemRequest request)
    {
        var response = await _mediator.Send(new AddItemToCartCommand(request));
        return Ok(response);
    }
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> RemoveItemFromCartAsync(long customerId, long itemId)
    {
        var response = await _mediator.Send(new DeleteItemFromCartCommand(customerId, itemId));
        return Ok(response);
    }
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> ClearCartAsync(long cartId)
    {
        var response = await _mediator.Send(new ClearCartCommand(cartId));
        return Ok(response);
    }
    [Authorize]
    [HttpPatch]
    public async Task<IActionResult> UpdateQuantityAsync(long customerId, long cartItemId, int quantity)
    {
        var response = await _mediator.Send(new UpdateQuantityCartItemCommand(customerId, cartItemId, quantity));
        return Ok(response);
    }
}