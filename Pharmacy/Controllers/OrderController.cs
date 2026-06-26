using Microsoft.AspNetCore.Mvc;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController:ControllerBase
{
    private readonly IOrderService _orderService;
    public  OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }   
    
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] OrderRequest request)
    {
        var response =await  _orderService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateReservationOrderAsync(OrderReservationRequest reservationRequest)
    {
        var response=await _orderService.CreateReservationOrderAsync(reservationRequest);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<OrderResponse>> UpdateOrder(long id, [FromBody] OrderRequest request)
    {
        try
        {
        var response =await _orderService.UpdateAsync(id,request);
        return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("id")]
    public async Task<ActionResult<OrderResponse>> GetOrderById(long id)
    {
        try
        {
        var response =await _orderService.GetByIdAsync(id);
        return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAllOrderByPagenation(int pageNumber, int pageSize)
    {
        try
        {
        var response=await _orderService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOrderById(long id)
    {
        try
        {
            
        var response =await _orderService.DeleteAsync(id);
        return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> AddItemToOrderAsync(long orderId, OrderItemRequest itemrequest)
    {
        try
        {
        var response = await _orderService.AddItemToOrderAsync(orderId, itemrequest);
        return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<OrderResponse>> RemoveItemFromOrderAsync(long orderId, long orderItemId)
    {
        try
        {
        var response = await _orderService.RemoveItemFromOrderAsync(orderId, orderItemId);
        return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }
    
}