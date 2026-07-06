using Microsoft.AspNetCore.Mvc;
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
        var response = await _productService.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<ProductResponse>> UpdateProduct(long id, [FromBody] ProductRequest request)
    {
        var response = await _productService.UpdateAsync(id, request);
        return Ok(response);
    }

    [HttpGet("id")]
    public async Task<ActionResult<ProductResponse>> GetProductById(long id)
    {
        var response = await _productService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAllProductsByPagenation(int pageNumber, int pageSize)
    {
        var response = await _productService.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProductById(long id)
    {
        var response = await _productService.DeleteAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductResponse>> GetProductByBarcodeAsync(string barcode)
    {
        var response = await _productService.GetProductByBarcodeAsync(barcode);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByNameAsync(string name)
    {
        var response = await _productService.GetProductsByNameAsync(name);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByCategoryIdAsync(long categoryId, int page,
        int pageSize)
    {
        var response = await _productService.GetProductsByCategoryIdAsync(categoryId, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetOutOfStockAsync(int page, int pageSize)
    {
        var response = await _productService.GetOutOfStockAsync(page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetLowOfStockAsync(int minquantity, int page, int pageSize)
    {
        var response = await _productService.GetLowOfStockAsync(minquantity, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByPurchasePriceAsync(decimal price, int page,
        int pageSize)
    {
        var response = await _productService.GetProductsByPurchasePriceAsync(price, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByOrderPriseAsync(decimal price, int page,
        int pageSize)
    {
        var response = await _productService.GetProductsByOrderPriseAsync(price, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByCountryAsync(CountryEnum country, int page,
        int pageSize)
    {
        var response = await _productService.GetProductsByCountryAsync(country, page, pageSize);
        return Ok(response);
    }
}