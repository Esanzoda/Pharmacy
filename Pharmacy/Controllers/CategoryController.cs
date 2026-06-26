using Microsoft.AspNetCore.Mvc;
using Pharmasy.Exeption;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;
//scqlqr
namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryServise _categoryServise;

    public CategoryController(ICategoryServise categoryServise)
    {
        _categoryServise = categoryServise;
    }

    [HttpPost("dtefrted")]
    public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CategoryRequest request)
    {
        var response = await _categoryServise.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CategoryResponse>> UpdateCategory(long id, [FromBody] CategoryRequest request)
    {
        try
        {
            var response = await _categoryServise.UpdateAsync(id, request);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpGet]
    public async Task<ActionResult<CategoryResponse>> GetCategoryById(long id)
    {
        try
        {
            var response = await _categoryServise.GetByIdAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllCategoriesByPagenation(int pageNumber, int pageSize)
    {
        try
        {
            var response = await _categoryServise.GetAllByPaginationAsync(pageNumber, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategoryById(long id)
    {
        try
        {
            var response = await _categoryServise.DeleteAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetCategoryWithProducts(int categoryId, int page,
        int pageSize)
    {
        try
        {
            var response = await _categoryServise.GetCategoryWithProducts(categoryId, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> SearchByNameAsync(string name)
    {
        try
        {
            var response = await _categoryServise.SearchByNameAsync(name);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetActiveCategoriesAsync()
    {
        try
        {
            var response = await _categoryServise.GetActiveCategoriesAsync();
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<bool>> GetCategoryExistsAsync(string name)
    {
        try
        {
            var response = await _categoryServise.CategoryExistsAsync(name);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }
}