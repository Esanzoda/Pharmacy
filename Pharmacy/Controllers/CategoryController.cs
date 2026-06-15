using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;
using Pharmasy.Services;

namespace Pharmasy.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryController:ControllerBase
{
    private readonly ICategoryServise _categoryServise;

    public CategoryController(ICategoryServise categoryServise)
    {
        _categoryServise = categoryServise;
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CategoryRequest request)
    {
        var response =await  _categoryServise.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CategoryResponse>> UpdateCategory([FromBody] long id, CategoryRequest request)
    {
        var response =await _categoryServise.UpdateAsync(id,request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CategoryResponse>> GetCategoryById(long id)
    {
        var response =await _categoryServise.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllCategoriesByPagenation(int pageNumber, int pageSize)
    {
        var response=_categoryServise.GetAllByPaginationAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategoryById(long id)
    {
        var response =await _categoryServise.DeleteAsync(id);
        return Ok(response);
    }
    
}