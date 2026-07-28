using AlHudhud.Services.ScopesOfWorkService;
using BestPriceStore.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlHudhud.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Viewer")]
public class ScopeOfWorkController : ControllerBase
{
    private readonly IScopesOfWorkService _scopesService;

    public ScopeOfWorkController(IScopesOfWorkService scopesService)
    {
        _scopesService = scopesService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ScopeOfWorkResponseDTO>>>> GetScopes()
    {
        var response = await _scopesService.GetAllScopesAsync();
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ScopeOfWorkResponseDTO>>> GetScopeDetails(int id)
    {
        var response = await _scopesService.GetScopeByIdAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ScopeOfWorkResponseDTO>>> CreateScope([FromBody] CreateScopeOfWorkRequestDTO createScopeDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ScopeOfWorkResponseDTO>(400, "Invalid scope data."));
        }
        var response = await _scopesService.CreateScopeAsync(createScopeDTO);
        if (response.StatusCode != 201)
        {
            return StatusCode(response.StatusCode, response);
        }
        return StatusCode(201, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ScopeOfWorkResponseDTO>>> UpdateScope(int id, [FromBody] UpdateScopeOfWorkRequestDTO updateScopeDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ScopeOfWorkResponseDTO>(400, "Invalid scope data."));
        }
        var response = await _scopesService.UpdateScopeAsync(id, updateScopeDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteScope(int id)
    {
        var response = await _scopesService.DeleteScopeAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }
}
