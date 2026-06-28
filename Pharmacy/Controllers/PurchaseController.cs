using Microsoft.AspNetCore.Mvc;
using Pharmasy.Exeption;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;
 
    public PurchaseController(IPurchaseService productService)
    {
        _purchaseService = productService;
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseResponse>> CreatePurchase([FromBody] PurchaseRequest request)
    {
        var response = await _purchaseService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<PurchaseResponse>> UpdatePurchase(long id, [FromBody] PurchaseRequest request)
    {
        try
        {
            var response = await _purchaseService.UpdateAsync(id, request);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("id")]
    public async Task<ActionResult<PurchaseResponse>> GetPurchaseById(long id)
    {
        try
        {
            var response = await _purchaseService.GetByIdAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetAllPurchasesByPagenation(int pageNumber, int pageSize)
    {
        try
        {
            var response = await _purchaseService.GetAllByPaginationAsync(pageNumber, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePurchaseById(long id)
    {
        try
        {
            var response = await _purchaseService.DeleteAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseItemResponse>> AddItemToPurchase(long purchaseId,
        PurchaseItemRequest purchaserequest)
    {
        try
        {
            var resourse = await _purchaseService.AddItemToPurchase(purchaseId, purchaserequest);
            return Ok(resourse);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<PurchaseItemResponse>> RemoveItemFromPurchase(long purchaseId, long purchaseItemId)
    {
        try
        {
            var response = await _purchaseService.RemoveItemFromPurchase(purchaseId, purchaseItemId);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }
}