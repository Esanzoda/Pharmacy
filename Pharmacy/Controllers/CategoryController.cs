using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Category.Commands;
using Pharmacy.CQRS.Category.Queries;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create([FromBody] CreateCategoryRequest request)
    {
        var response = await mediator.Send(new CreateCategoryCommand(request));
        return Ok(response);
    }

    [Authorize(Roles = nameof(Role.Admin))]
    [HttpPut]
    public async Task<ActionResult<CategoryResponse>> Update(long id, [FromBody] UpdateCategoryRequest request)
    {
        var response = await mediator.Send(new UpdateCategoryCommand(id, request));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CategoryResponse>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return Ok(response);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllByPaginationAsync(int page, int pageSize)
    {
        var response = await mediator.Send(new GetAllCategoriesByPaginationQuery(page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await mediator.Send(new DeleteCategoryCommand(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetWithProducts(int categoryId, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetCategoryByIdWithProductsQuery(categoryId, page, pageSize));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> SearchByNameAsync(string name)
    {
        var response = await mediator.Send(new GetByNameQuery(name));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetActiveAsync(int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetActiveCategoriesQuery(pageNumber, pageSize));
        return Ok(response);
    }
}