using AlHudhud.Services.ProposalsService;
using AlHudhud.DTOs.Proposals;
using AlHudhud.DTOs.Common;
using BestPriceStore.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlHudhud.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Viewer")]
public class ProposalsController : ControllerBase
{
    private readonly IProposalsService _proposalsService;

    public ProposalsController(IProposalsService proposalsService)
    {
        _proposalsService = proposalsService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResultDTO<ProposalResponseDTO>>>> GetAllProposals([FromQuery] PaginationParametersDTO pagination)
    {
        var response = await _proposalsService.GetAllProposalsAsync(pagination);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> CreateProposal([FromBody] CreateProposalRequestDTO createProposalDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ConfirmationResponseDTO>(400, "Invalid proposal data."));
        }

        var response = await _proposalsService.CreateProposalAsync(createProposalDTO);
        if (response.StatusCode != 201)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateProposal(int id, [FromBody] UpdateProposalRequestDTO updateProposalDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ConfirmationResponseDTO>(400, "Invalid proposal data."));
        }

        var response = await _proposalsService.UpdateProposalAsync(id, updateProposalDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> ChangeProposalStatus(int id, [FromBody] ChangeProposalStatusDTO statusDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ConfirmationResponseDTO>(400, "Invalid status data."));
        }

        var response = await _proposalsService.ChangeProposalStatusAsync(id, statusDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPost("{id}/version")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> CreateProposalVersion(int id, [FromBody] CreateProposalVersionRequestDTO versionDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ConfirmationResponseDTO>(400, "Invalid version data."));
        }

        var response = await _proposalsService.CreateProposalVersionAsync(id, versionDTO);
        if (response.StatusCode != 201)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpGet("{id}/history")]
    [Authorize(Roles = "Admin,Viewer")]
    public async Task<ActionResult<ApiResponse<List<ProposalResponseDTO>>>> GetProposalHistory(int id)
    {
        var response = await _proposalsService.GetProposalHistoryAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }
}
