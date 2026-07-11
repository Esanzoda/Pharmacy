using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;
using Pharmasy.Services.Product.Query;
using Pharmasy.Services.Purchase.Command;
using Pharmasy.Services.Purchase.Query;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;
    private readonly IMediator _mediator;

    public PurchaseController(IPurchaseService productService)
    {
        _purchaseService = productService;
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseResponse>> Create([FromBody] PurchaseRequest request)
    {
        var response = await _mediator.Send(new CreatePurchaseCommand(request));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<PurchaseResponse>> Update(long id, [FromBody] PurchaseRequest request)
    {
        var response = await _purchaseService.UpdateAsync(id, request);
        return Ok(response);
    }

    [HttpGet("id")]
    public async Task<ActionResult<PurchaseResponse>> GetByIdAsync(long id)
    {
        var response = await _mediator.Send(new GetProductByIdQuery(id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetAllByPagenation(int pageNumber, int pageSize)
    {
        var response = await _mediator.Send(new GetAllPurchaseQuery(pageNumber, pageSize));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await _mediator.Send(new DeletePurchaseCommand(id));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseItemResponse>> AddItem(long purchaseId,
        PurchaseItemRequest purchaserequest)
    {
        var resourse = await _mediator.Send(new AddItemToPurchaseCommand(purchaseId, purchaserequest));
        return Ok(resourse);
    }

    [HttpDelete]
    public async Task<ActionResult<PurchaseItemResponse>> RemoveItem(long employeeId,long purchaseId, long purchaseItemId)
    {
        var response = await _mediator.Send(new RemoveItemFromPurchaseCommandHandler(employeeId, purchaseId, purchaseItemId));
        return Ok(response);
    }
}