using AlHudhud.Services.ClientsService;
using BestPriceStore.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlHudhud.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Viewer")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;

    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ClientResponseDTO>>>> GetAllClients()
    {
        var response = await _clientsService.GetAllClientsAsync();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ClientResponseDTO>>> GetClientDetails(int id)
    {
        var response = await _clientsService.GetClientByIdAsync(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ClientResponseDTO>>> CreateClient([FromBody] CreateClientRequestDTO createClientDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ClientResponseDTO>(400, "Invalid client data."));
        }
        var response = await _clientsService.CreateClientAsync(createClientDTO);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ClientResponseDTO>>> UpdateClient(int id, [FromBody] UpdateClientRequestDTO updateClientDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ClientResponseDTO>(400, "Invalid client data."));
        }
        var response = await _clientsService.UpdateClientAsync(id, updateClientDTO);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteClient(int id)
    {
        var response = await _clientsService.DeleteClientAsync(id);
        return StatusCode(response.StatusCode, response);
    }
}
