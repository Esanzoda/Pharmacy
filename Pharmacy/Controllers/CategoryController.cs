using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Category.Commands;
using Pharmacy.CQRS.Category.Queries;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

//scqlqr
namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController( IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create([FromBody] CreateCategoryRequest request)
    {
        var response = await _mediator.Send(new CreateCategoryCommand(request));
        return Ok(response);
    }
    [Authorize(Roles = nameof(Role.Admin) )]
    [HttpPut]
    public async Task<ActionResult<CategoryResponse>> Update(long id, [FromBody] UpdateCategoryRequest request)
    {
        var response = await _mediator.Send(new UpdateCategoryCommand(id, request));
        return Ok(response);
    }
    [Authorize]

    [HttpGet]
    public async Task<ActionResult<CategoryResponse>> GetById(long id,CancellationToken cancellationToken=default)
    {
        var response = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    // no recomendate for from body in get 
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllByPagenationAsinc( int page,int pageSize)
    {
        var response = await _mediator.Send(new  GetAllCategoriesByPeginationQuery (page,pageSize));
        return Ok(response);
    }
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await _mediator.Send(new DeleteCategoryCommand(id));
        return Ok(response);
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetWithProducts(int categoryId, int page,
        int pageSize)
    {
        var response = await _mediator.Send(new GetCategoryByIdWithProductsQuery(categoryId, page, pageSize));
        return Ok(response);
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> SearchByNameAsync(string name)
    {
        var response = await _mediator.Send(new GetByNameQuery(name));
        return Ok(response);
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetActiveAsync(int pageNumber,int pageSize)
    {
        var response = await _mediator.Send(new GetActiveCategoriesQuery(pageNumber,pageSize));
        return Ok(response);
    }
}