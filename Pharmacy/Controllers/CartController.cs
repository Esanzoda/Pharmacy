using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using MediatR;
using Pharmasy.Services.Cart.Command;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController( IMediator mediator)
    {
        
        _mediator = mediator;
    }


    [HttpPut("idid")]
    public async Task<IActionResult> UpdateItemQuantity(long customerId,long cartItemId,int quantity)
    {
        var response = await _mediator.Send(new UpdateQuantityCartItemCommand(customerId, cartItemId, quantity));
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCartAsync(CartItemRequest request)
    {
        var response = await _mediator.Send(new AddItemToCartCommand(request));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveItemFromCartAsync(long customerId, long itemId)
    {
        var response = await _mediator.Send(new DeleteItemFromCartCommand(customerId, itemId));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCartAsync(long cartId)
    {
        var response = await _mediator.Send(new ClearCartCommand(cartId));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateQuantityAsync(long customerId, long cartItemId, int quantity)
    {
        var response = await _mediator.Send(new UpdateQuantityCartItemCommand(customerId, cartItemId, quantity));
        return Ok(response);
    }
}