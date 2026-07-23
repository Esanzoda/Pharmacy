using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Purchase.Commands;
using Pharmacy.CQRS.Purchase.Queries;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PurchaseController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PurchaseResponse>> Create([FromBody] PurchaseRequest request)
    {
        var response = await mediator.Send(new CreatePurchaseCommand(request));
        return Ok(response);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<PurchaseResponse>> Update(long id, [FromBody] PurchaseRequest request)
    {
        var response = await mediator.Send(new UpdatePurchaseCommand(id, request));
        return Ok(response);
    }

    [Authorize]
    [HttpGet("id")]
    public async Task<ActionResult<PurchaseResponse>> GetByIdAsync(long id)
    {
        var response = await mediator.Send(new GetPurchaseBuIdQuery(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<PurchaseResponse>>> GetAllByPagination(int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllPurchaseQuery(pageNumber, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await mediator.Send(new DeletePurchaseCommand(id));
        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PurchaseItemResponse>> AddItem(long purchaseId,
        PurchaseItemRequest purchaseItemRequest)
    {
        var response = await mediator.Send(new AddItemToPurchaseCommand(purchaseId, purchaseItemRequest));
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<PurchaseItemResponse>> RemoveItem(long employeeId, long purchaseId,
        long purchaseItemId)
    {
        var response = await mediator.Send(new RemoveItemFromPurchaseCommand(employeeId, purchaseId, purchaseItemId));
        return Ok(response);
    }
}