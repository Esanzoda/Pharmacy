using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Order.Commands;
using Pharmacy.CQRS.Order.Queries;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] OrderRequest request)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new CreateOrderCommand(customerId, request));
        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateFromCart(OrderType orderType, string address)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new CreateOrderFromCartCommand(customerId, orderType, address));
        return Ok(response);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<OrderResponse>> UpdateStatusAsync(long id, [FromBody] UpdateOrderRequest request)
    {
        var response = await mediator.Send(new UpdateOrderStatusCommand(id, request));
        return Ok(response);
    }

    [Authorize]
    [HttpGet("id")]
    public async Task<ActionResult<OrderResponse>> GetByIdAsync(long id)
    {
        var response = await mediator.Send(new GetOrderByIdQuery(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAllByPagination(int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllOrdersQuery(pageNumber, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetByStatusAsync(OrderStatus status, int pageNumber,
        int pageSize)
    {
        var response = await mediator.Send(new GetOrderByOrderStatusQuery(status, pageNumber, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await mediator.Send(new DeleteOrderCommand(id));
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<OrderResponse>> RemoveItemFromAsync(long orderId, long orderItemId)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new RemoveItemFromOrderCommand(customerId, orderId, orderItemId));
        return Ok(response);
    }
}