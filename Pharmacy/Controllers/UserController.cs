using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Services;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Register([FromBody] UserRequest request)
    {
        try
        {
            var response = await _userService.CreateAsync(request);
            return Ok(response);
        }
        catch (ResourseIsAlredyExsistExeption ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
 
    public async Task<ActionResult<UserResponse>> GetById(long id)
    {
        try
        {
            var response = await _userService.GetByIdAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetAllByPage(int pageNumber, int pageSize)
    {
        try
        {
            var response = await _userService.GetAllByPaginationAsync(pageNumber, pageSize);
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
     
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetByRole(Role role, int page, int pageSize)
    {
        try
        {
            var response = await _userService.GetByRoleAsync(role, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]

    public async Task<ActionResult<List<UserResponse>>> GetByName(string name, int page, int pageSize)
    {
        try
        {
            var response = await _userService.GetByNameAsync(name, page, pageSize);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
  
    public async Task<ActionResult<UserResponse>> Update(long id, [FromBody] UserRequest request)
    {
        try
        {
            var response = await _userService.UpdateAsync(id, request);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
 
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var response = await _userService.DeleteAsync(id);
            return Ok(response);
        }
        catch (ResourseNotFoundExeption ex)
        {
            return NotFound(ex.Message);
        }
    }
}