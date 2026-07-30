using AlHudhud.Services.ProposalsService;
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
    public async Task<ActionResult<ApiResponse<IEnumerable<ProposalResponseDTO>>>> GetProposals()
    {
        var response = await _proposalsService.GetAllProposalsAsync();
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProposalDetailsWithHistoryResponseDTO>>> GetProposalDetails(int id)
    {
        var response = await _proposalsService.GetProposalByIdAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ProposalResponseDTO>>> CreateProposal([FromBody] CreateProposalRequestDTO createProposalDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ProposalResponseDTO>(400, "Invalid proposal data."));
        }
        var response = await _proposalsService.CreateProposalAsync(createProposalDTO);
        if (response.StatusCode != 201)
        {
            return StatusCode(response.StatusCode, response);
        }
        return StatusCode(201, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ProposalResponseDTO>>> UpdateProposal(int id, [FromBody] UpdateProposalRequestDTO updateProposalDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ProposalResponseDTO>(400, "Invalid proposal data."));
        }
        var response = await _proposalsService.UpdateProposalAsync(id, updateProposalDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPost("{id}/version")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ProposalResponseDTO>>> CreateProposalVersion(int id, [FromBody] CreateProposalVersionRequestDTO versionDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ProposalResponseDTO>(400, "Invalid version data."));
        }
        var response = await _proposalsService.CreateProposalVersionAsync(id, versionDTO);
        if (response.StatusCode != 201)
        {
            return StatusCode(response.StatusCode, response);
        }
        return StatusCode(201, response);
    }

    [HttpPatch("{id}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ProposalResponseDTO>>> ApproveProposal(int id)
    {
        var response = await _proposalsService.ApproveProposalAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPatch("{id}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ProposalResponseDTO>>> RejectProposal(int id)
    {
        var response = await _proposalsService.RejectProposalAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }
}
