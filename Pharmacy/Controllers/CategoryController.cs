
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;
using Pharmasy.Services.Category.Command;
using Pharmasy.Services.Category.Query;

//scqlqr
namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(ICategoryServise categoryServise, IMediator mediator)
    {
        _mediator = mediator;
    }

   
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create([FromBody] CategoryRequest request)
    {
        var response = await _mediator.Send(new CreateCategoryCommand(request));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CategoryResponse>> Update(long id, [FromBody] CategoryRequest request)
    {
        var response = await _mediator.Send(new UpdateCategoryCommand(id, request));
        return Ok(response);
    }


    [HttpGet]
    public async Task<ActionResult<CategoryResponse>> GetById(long id,CancellationToken cancellationToken=default)
    {
        var response = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllByPagenationAsinc(int pageNumber, int pageSize)
    {
        var response = await _mediator.Send(new GetActiveCategoriesQuery(), CancellationToken.None);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await _mediator.Send(new DeleteCategoryCommand(id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetWithProducts(int categoryId, int page,
        int pageSize)
    {
        var response = await _mediator.Send(new GetCategoryByIdWithProductsQuery(categoryId, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> SearchByNameAsync(string name)
    {
        var response = await _mediator.Send(new SerchByNameQuery(name));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetActiveAsync()
    {
        var response = await _mediator.Send(new GetActiveCategoriesQuery());
        return Ok(response);
    }
}