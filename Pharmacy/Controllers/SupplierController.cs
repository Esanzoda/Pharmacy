using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;
[ApiController]
[Route("api/supplier/[controller]")]
public class SupplierController:ControllerBase
{
   private readonly ISupplierService _supplierService;

   public SupplierController(ISupplierService supplierService)
   {
      _supplierService = supplierService;
   }
   
   [HttpPost]
   public async Task<ActionResult<SupplierResponse>> CreateCategory([FromBody] SupplierRequest request)
   {
      var response =await  _supplierService.CreateAsync(request);
      return Ok(response);
   }

   [HttpPut]
   public async Task<ActionResult<SupplierResponse>> UpdateCategory([FromBody] long id, SupplierRequest request)
   {
      var response =await _supplierService.UpdateAsync(id,request);
      return Ok(response);
   }

   [HttpGet]
   public async Task<ActionResult<SupplierResponse>> GetCategoryById(long id)
   {
      var response =await _supplierService.GetByIdAsync(id);
      return Ok(response);
   }

   [HttpGet]
   public async Task<ActionResult<List<SupplierResponse>>> GetAllCategoriesByPagenation(int pageNumber, int pageSize)
   {
      var response=_supplierService.GetAllByPaginationAsync(pageNumber, pageSize);
      return Ok(response);
   }

   [HttpDelete]
   public async Task<IActionResult> DeleteCategoryById(long id)
   {
      var response =await _supplierService.DeleteAsync(id);
      return Ok(response);
   }
}