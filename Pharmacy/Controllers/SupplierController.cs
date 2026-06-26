using Microsoft.AspNetCore.Mvc;
using Pharmasy.Exeption;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpPost]
    public async Task<ActionResult<SupplierResponse>> CreateSupplier([FromBody] SupplierRequest request)
    {
        var response = await _supplierService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<SupplierResponse>> UpdateCSupplier(long id, [FromBody] SupplierRequest request)
    {
        try
        {
            var response = await _supplierService.UpdateAsync(id, request);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("id")]
    public async Task<ActionResult<SupplierResponse>> GetSupplierById(long id)
    {
        try
        {
            var response = await _supplierService.GetByIdAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<SupplierResponse>>> GetAllSuppliersByPagenation(int pageNumber, int pageSize)
    {
        try
        {
            var response = await _supplierService.GetAllByPaginationAsync(pageNumber, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteSupplierById(long id)
    {
        try
        {
            var response = await _supplierService.DeleteAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }
}