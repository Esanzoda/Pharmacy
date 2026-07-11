using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using MediatR;
using Pharmasy.Services;
using Pharmasy.Services.Cart.Command;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly ICartItemService _cartItemService;
    private readonly IMediator _mediator;

    public CartController(ICartService cartService, ICartItemService cartItemService, IMediator mediator)
    {
        _cartService = cartService;
        _cartItemService = cartItemService;
        _mediator = mediator;
    }


    [HttpPut]
    public async Task<IActionResult> UpdateCart(long id, [FromBody] CartRequest request)
    {
        var response = await _cartService.UpdateAsync(id, request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCartAsync(CartItemRequest request)
    {
        var response = await _cartItemService.AddItemToCartAsync(request);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveItemFromCartAsync(long customerId, long itemId)
    {
        var response = await _cartItemService.DeleteItemFromCartAsync(customerId, itemId);
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
        var response = await _cartItemService.UpdateQuantityCartItem(customerId, cartItemId, quantity);
        return Ok(response);
    }
}