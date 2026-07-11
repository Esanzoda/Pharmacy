using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DeliverController : ControllerBase
{
    private readonly IDelivererService _deliverService;

    public DeliverController(IDelivererService deliverService)
    {
        _deliverService = deliverService;
    }

    [HttpPost]
    public async Task<ActionResult<DeliverResponse>> CreateDeliver([FromBody] DeliverRequest request)
    {
        var response = await _deliverService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> UpdateDeliver(long id, [FromBody] DeliverRequest request)
    {
        var response = await _deliverService.UpdateAsync(id, request);
        return Ok(response);
    }

    [HttpGet("id")]
    public async Task<ActionResult<DeliverResponse>> GetDeliverById(long id)
    {
        var response = await _deliverService.GetByIdAsync(id, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<DeliverResponse>>> GetAllDekiversByPagenation(int pageNumber, int pageSize)
    {
        var response = await _deliverService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDeliverById(long id)
    {
        var response = await _deliverService.DeleteAsync(id);
        return Ok(response);
    }
}