using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class PurchaseController:ControllerBase
{
    private readonly IPurchaseService _purchaseService;
    public  PurchaseController(IPurchaseService productService)
    {
      _purchaseService = productService;
    }
    
    [HttpPost]
    public async Task<ActionResult<PurchaseResponse>> CreateCategory([FromBody] PurchaseRequest request)
    {
        var response =await  _purchaseService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<PurchaseResponse>> UpdateCategory([FromBody] long id, PurchaseRequest request)
    {
        var response =await _purchaseService.UpdateAsync(id,request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<PurchaseResponse>> GetCategoryById(long id)
    {
        var response =await _purchaseService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetAllCategoriesByPagenation(int pageNumber, int pageSize)
    {
        var response=_purchaseService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategoryById(long id)
    {
        var response =await _purchaseService.DeleteAsync(id);
        return Ok(response);
    }
}