using Microsoft.AspNetCore.Mvc;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] OrderRequest request)
    {
        var response = await _orderService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<OrderResponse>> UpdateOrder(long id, [FromBody] UpdateOrderRequest request)
    {
        var response = await _orderService.UpdateOrderStatusAsync(id, request);
        return Ok(response);
    }

    [HttpGet("id")]
    public async Task<ActionResult<OrderResponse>> GetOrderById(long id)
    {
        var response = await _orderService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAllOrderByPagenation(int pageNumber, int pageSize)
    {
        var response = await _orderService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOrderById(long id)
    {
        var response = await _orderService.DeleteAsync(id);
        return Ok(response);
    }


    [HttpDelete]
    public async Task<ActionResult<OrderResponse>> RemoveItemFromOrderAsync(long orderId, long orderItemId)
    {
        var response = await _orderService.RemoveItemFromOrderAsync(orderId, orderItemId);
        return Ok(response);
    }
}