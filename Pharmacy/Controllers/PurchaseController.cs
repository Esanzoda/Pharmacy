using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Purchase.Commands;
using Pharmacy.CQRS.Purchase.Queries;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;
using Pharmacy.CQRS.Product.Queries;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PurchaseController : ControllerBase
{
 
    private readonly IMediator _mediator;

    public PurchaseController( IMediator mediator)
    {
       
        _mediator = mediator;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PurchaseResponse>> Create([FromBody] PurchaseRequest request)
    {
        var response = await _mediator.Send(new CreatePurchaseCommand(request));
        return Ok(response);
    }
    [Authorize]
    [HttpPut]

    public async Task<ActionResult<PurchaseResponse>> Update(long id, [FromBody] PurchaseRequest request)
    {
        var response = await _mediator.Send(new UpdatePurchaseCommand(id, request));
        return Ok(response);
    }
    [Authorize]
    [HttpGet("id")]
    public async Task<ActionResult<PurchaseResponse>> GetByIdAsync(long id)
    {
        var response = await _mediator.Send(new GetPurchaseBuIdQuery(id));
        return Ok(response);
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetAllByPagenation(int pageNumber, int pageSize)
    {
        var response = await _mediator.Send(new GetAllPurchaseQuery(pageNumber, pageSize));
        return Ok(response);
    }
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await _mediator.Send(new DeletePurchaseCommand(id));
        return Ok(response);
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PurchaseItemResponse>> AddItem(long purchaseId,
        PurchaseItemRequest purchaserequest)
    {
        var resourse = await _mediator.Send(new AddItemToPurchaseCommand(purchaseId, purchaserequest));
        return Ok(resourse);
    }
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<PurchaseItemResponse>> RemoveItem(long employeeId,long purchaseId, long purchaseItemId)
    {
        var response = await _mediator.Send(new RemoveItemFromPurchaseCommand(employeeId, purchaseId, purchaseItemId));
        return Ok(response);
    }
}