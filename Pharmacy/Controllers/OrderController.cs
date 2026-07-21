using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.CQRS.Order.Commands;
using Pharmasy.CQRS.Order.Queries;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController : ControllerBase
{
   
   private readonly IMediator _mediator;

    public OrderController( IMediator mediator)
    {
      
        _mediator = mediator;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] OrderRequest request)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _mediator.Send(new CreateOrderCommand(customerId,request));
        return Ok(response);
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateFromCart(OrderType orderType,string address)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await _mediator.Send(new CreateOrderFromCartCommand(customerId, orderType, address));
        return Ok(response);
    }
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<OrderResponse>> UpdateStatusAsync(long id, [FromBody] UpdateOrderRequest request)
    {
        var response = await _mediator.Send(new UpdateOrderStatusCommand(id, request));
        return Ok(response);
    }
    [Authorize]
    [HttpGet("id")]
    public async Task<ActionResult<OrderResponse>> GetByIdAsync(long id)
    {
        var response = await _mediator.Send(new GetOrderByIdQuery(id));
        return Ok(response);
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAllByPagenation(int pageNumber, int pageSize)
    {
        var response = await _mediator.Send(new GetAllOrdersQuery(pageNumber, pageSize));
        return Ok(response);
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetByStatusAsync(OrderStatus status, int pageNumber, int pageSize)
    {
        var response=await _mediator.Send(new GetOrderByOrderStatusQuery(status, pageNumber, pageSize));
        return Ok(response);
    }
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response =await _mediator.Send(new DeleteOrderCommand(id));
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<OrderResponse>> RemoveItemFromAsync(long orderId, long orderItemId)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _mediator.Send(new RemoveItemFromOrderCommand(customerId,orderId, orderItemId));
        return Ok(response);
    }
}