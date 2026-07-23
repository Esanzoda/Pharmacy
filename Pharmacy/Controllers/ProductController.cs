using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Product.Commands;
using Pharmacy.CQRS.Product.Queries;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] ProductRequest request)
    {
        var response = await mediator.Send(new CreateProductCommand(request));
        return Ok(response);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ProductResponse>> Update(long id, [FromBody] ProductRequest request)
    {
        var response = await mediator.Send(new UpdateProductCommand(id, request));
        return Ok(response);
    }

    [Authorize]
    [HttpGet("id")]
    public async Task<ActionResult<ProductResponse>> GetById(long id)
    {
        var response = await mediator.Send(new GetProductByIdQuery(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAllByPagination(int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllProductsQuery(pageNumber, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await mediator.Send(new DeleteProductCommand(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ProductResponse>> GetByBarcodeAsync(string barcode)
    {
        var response = await mediator.Send(new GetProductByBarcodeQuery(barcode));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByNameAsync(string name, int page, int pageSize)
    {
        var response = await mediator.Send(new GetProductsByNameQuery(name, page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByCategoryIdAsync(long categoryId, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByCategoryIdQuery(categoryId, page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetOutOfStockAsync(int page, int pageSize)
    {
        var response = await mediator.Send(new GetOutOfStockQuery(page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetLowOfStockAsync(int minquantity, int page, int pageSize)
    {
        var response = await mediator.Send(new GetLowOfStockQuery(minquantity, page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByPurchasePriceAsync(decimal price, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByPurchasePriceQuery(price, page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByOrderPriseAsync(decimal price, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByOrderPriceQuery(price, page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByCountryAsync(CountryEnum country, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByCountryQuery(country, page, pageSize));
        return Ok(response);
    }
}