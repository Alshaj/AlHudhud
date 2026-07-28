using AlHudhud.Services.UsersService;
using BestPriceStore.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlHudhud.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Viewer")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserResponseDTO>>>> GetUsers()
    {
        var response = await _usersService.GetAllUsersAsync();
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UserResponseDTO>>> GetUserDetails(int id)
    {
        var response = await _usersService.GetUserByIdAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<UserResponseDTO>>> CreateUser([FromBody] CreateUserRequestDTO createUserDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<UserResponseDTO>(400, "Invalid user data."));
        }
        var response = await _usersService.CreateUserAsync(createUserDTO);
        if (response.StatusCode != 201)
        {
            return StatusCode(response.StatusCode, response);
        }
        return StatusCode(201, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<UserResponseDTO>>> UpdateUser(int id, [FromBody] UpdateUserRequestDTO updateUserDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<UserResponseDTO>(400, "Invalid user data."));
        }
        var response = await _usersService.UpdateUserAsync(id, updateUserDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<UserResponseDTO>>> ToggleUserStatus(int id, [FromBody] ChangeUserStatusDTO statusDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<UserResponseDTO>(400, "Invalid status data."));
        }
        var response = await _usersService.ToggleUserStatusAsync(id, statusDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteUser(int id)
    {
        var response = await _usersService.DeleteUserAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }
}
