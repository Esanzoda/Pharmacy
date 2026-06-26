using Microsoft.AspNetCore.Mvc;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] ProductRequest request)
    {
        try
        {
            var response = await _productService.CreateAsync(request);
            return Ok(response);
        }
        catch (ResourseIsAlredyExsistExeption ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        
    }

    [HttpPut]
    public async Task<ActionResult<ProductResponse>> UpdateOrder(long id, [FromBody] ProductRequest request)
    {
        try
        {
            var response = await _productService.UpdateAsync(id, request);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("id")]
    public async Task<ActionResult<ProductResponse>> GetOrderById(long id)
    {
        try
        {
            var response = await _productService.GetByIdAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAllOrdersByPagenation(int pageNumber, int pageSize)
    {
        try
        {
            var response = await _productService.GetAllByPaginationAsync(pageNumber, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOrderById(long id)
    {
        try
        {
            var response = await _productService.DeleteAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<ProductResponse>> GetProductByBarcodeAsync(string barcode)
    {
        try
        {
            var response = await _productService.GetProductByBarcodeAsync(barcode);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByNameAsync(string name)
    {
        try
        {
            var response = await _productService.GetProductsByNameAsync(name);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByCategoryIdAsync(long categoryId, int page,
        int pageSize)
    {
        try
        {
            var response = await _productService.GetProductsByCategoryIdAsync(categoryId, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetOutOfStockAsync(int page, int pageSize)
    {
        try
        {
            var response = await _productService.GetOutOfStockAsync(page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetLowOfStockAsync(int minquantity, int page, int pageSize)
    {
        try
        {
            var response = await _productService.GetLowOfStockAsync(minquantity, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetExpireDateAsync(int page, int pageSize)
    {
        try
        {
            var response = await _productService.GetExpireDateAsync(page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByPurchasePriceAsync(decimal price, int page,
        int pageSize)
    {
        try
        {
            var response = await _productService.GetProductsByPurchasePriceAsync(price, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByOrderPriseAsync(decimal price, int page,
        int pageSize)
    {
        try
        {
            var response = await _productService.GetProductsByOrderPriseAsync(price, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByCountryAsync(CountryEnum country, int page,
        int pageSize)
    {
        try
        {
            var response = await _productService.GetProductsByCountryAsync(country, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }
}