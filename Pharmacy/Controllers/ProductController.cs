using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]

public class ProductController:ControllerBase
{
   private readonly IProductService _productService;
   
   public ProductController(IProductService productService)
   {
      _productService = productService;
   }
   
   [HttpPost]
   public async Task<ActionResult<ProductResponse>> CreateCategory([FromBody] ProductRequest request)
   {
      var response =await  _productService.CreateAsync(request);
      return Ok(response);
   }

   [HttpPut]
   public async Task<ActionResult<ProductResponse>> UpdateCategory([FromBody] long id, ProductRequest request)
   {
      var response =await _productService.UpdateAsync(id,request);
      return Ok(response);
   }

   [HttpGet]
   public async Task<ActionResult<ProductResponse>> GetCategoryById(long id)
   {
      var response =await _productService.GetByIdAsync(id);
      return Ok(response);
   }

   [HttpGet]
   public async Task<ActionResult<List<ProductResponse>>> GetAllCategoriesByPagenation(int pageNumber, int pageSize)
   {
      var response=_productService.GetAllByPaginationAsync(pageNumber, pageSize);
      return Ok(response);
   }

   [HttpDelete]
   public async Task<IActionResult> DeleteCategoryById(long id)
   {
      var response =await _productService.DeleteAsync(id);
      return Ok(response);
   }
    
}