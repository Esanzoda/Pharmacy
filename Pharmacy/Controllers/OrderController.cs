using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services.Order.Command;
using Pharmasy.Services.Order.Query;

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

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] OrderRequest request)
    {
        var response = await _mediator.Send(new CreateOrderCommand(request));
        return Ok(response);
    }
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateFromCart(long customerId,OrderType orderType,string address)
    {
        var response = await _mediator.Send(new CreateOrderFromCartCommand(customerId, orderType, address));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<OrderResponse>> UpdateStatusAsync(long id, [FromBody] UpdateOrderRequest request)
    {
        var response = await _mediator.Send(new UpdateOrderStatusCommand(id, request));
        return Ok(response);
    }

    [HttpGet("id")]
    public async Task<ActionResult<OrderResponse>> GetByIdAsync(long id)
    {
        var response = await _mediator.Send(new GetOrderByIdQuery(id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAllByPagenation(int pageNumber, int pageSize)
    {
        var response = await _mediator.Send(new GetAllOrdersQuery(pageNumber, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetByStatusAsync(OrderStatus status, int pageNumber, int pageSize)
    {
        var response=await _mediator.Send(new GetOrderByOrderStatusQuery(status, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response =await _mediator.Send(new DeleteOrderCommand(id));
        return Ok(response);
    }


    [HttpDelete]
    public async Task<ActionResult<OrderResponse>> RemoveItemFromAsync(long orderId, long orderItemId)
    {
        var response = await _mediator.Send(new RemoveItemFromOrderCommand(orderId, orderItemId));
        return Ok(response);
    }
}