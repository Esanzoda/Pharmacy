using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrderController:ControllerBase
{
    private readonly IOrderService _orderService;
    public  OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }   
    
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateCategory([FromBody] OrderRequest request)
    {
        var response =await  _orderService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<OrderResponse>> UpdateCategory([FromBody] long id, OrderRequest request)
    {
        var response =await _orderService.UpdateAsync(id,request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<OrderResponse>> GetCategoryById(long id)
    {
        var response =await _orderService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAllCategoriesByPagenation(int pageNumber, int pageSize)
    {
        var response=_orderService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategoryById(long id)
    {
        var response =await _orderService.DeleteAsync(id);
        return Ok(response);
    }
    
}