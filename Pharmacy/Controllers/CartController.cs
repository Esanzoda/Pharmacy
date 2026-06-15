using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class CartController:ControllerBase
{
    private readonly ICartService _cartService;
    [HttpPost]
    public async Task<ActionResult<CartResponse>> CreateCart([FromBody] CartRequest request)
    {
        var response = await _cartService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CartResponse>> UpdateCart([FromBody]long id, CartRequest request)
    {
       var response= await _cartService.UpdateAsync(id,request); 
       return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CartResponse>>> GetAllCartsByPagination(int pageNumber, int pageSize)
    {
        var response=await _cartService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }
   
}