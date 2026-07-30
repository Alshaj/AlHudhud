using AlHudhud.Data;
using AlHudhud.Models;
using BestPriceStore.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AlHudhud.Services.ProposalsService;

public class ProposalsService : IProposalsService
{
    private readonly ApplicationDbContext _context;

    public ProposalsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<IEnumerable<ProposalResponseDTO>>> GetAllProposalsAsync()
    {
        // Return latest version of each proposal number
        var proposalsList = await _context.Proposals
            .Include(p => p.ProjectScope)
                .ThenInclude(ps => ps!.Project)
                    .ThenInclude(proj => proj!.Client)
            .Include(p => p.ProjectScope)
                .ThenInclude(ps => ps!.ScopeOfWork)
            .Include(p => p.ProposalStatus)
            .Include(p => p.ReferedByUser)
            .ToListAsync();

        var proposals = proposalsList
            .GroupBy(p => p.ProposalNumber)
            .Select(g => g.OrderByDescending(p => p.VersionNumber).First())
            .Select(p => new ProposalResponseDTO
            {
                Id = p.Id,
                ProposalNumber = p.ProposalNumber,
                ProjectScopeId = p.ProjectScopeId,
                ProjectName = p.ProjectScope?.Project?.Name ?? string.Empty,
                ClientName = p.ProjectScope?.Project?.Client?.ClientName ?? string.Empty,
                StatusId = p.StatusId,
                StatusName = p.ProposalStatus?.Name ?? "Unknown",
                ReferedBy = p.ReferedBy,
                ReferedByUserName = p.ReferedByUser?.UserName ?? "Unknown",
                Price = p.Price,
                Vat = p.Vat,
                ReceivedFromClient = p.ReceivedFromClient,
                PendingAmount = p.PendingAmount,
                VersionNumber = p.VersionNumber,
                Notes = p.Notes
            })
            .ToList();

        return new ApiResponse<IEnumerable<ProposalResponseDTO>>(200, proposals);
    }

    public async Task<ApiResponse<ProposalDetailsWithHistoryResponseDTO>> GetProposalByIdAsync(int id)
    {
        var p = await _context.Proposals
            .Include(x => x.ProjectScope)
                .ThenInclude(ps => ps!.Project)
                    .ThenInclude(proj => proj!.Client)
            .Include(x => x.ProjectScope)
                .ThenInclude(ps => ps!.ScopeOfWork)
            .Include(x => x.ProposalStatus)
            .Include(x => x.ReferedByUser)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (p == null)
        {
            return new ApiResponse<ProposalDetailsWithHistoryResponseDTO>(404, "Proposal not found.");
        }

        var detailsDto = new ProposalResponseDTO
        {
            Id = p.Id,
            ProposalNumber = p.ProposalNumber,
            ProjectScopeId = p.ProjectScopeId,
            ProjectName = p.ProjectScope?.Project?.Name ?? string.Empty,
            ClientName = p.ProjectScope?.Project?.Client?.ClientName ?? string.Empty,
            StatusId = p.StatusId,
            StatusName = p.ProposalStatus?.Name ?? "Unknown",
            ReferedBy = p.ReferedBy,
            ReferedByUserName = p.ReferedByUser?.UserName ?? "Unknown",
            Price = p.Price,
            Vat = p.Vat,
            ReceivedFromClient = p.ReceivedFromClient,
            PendingAmount = p.PendingAmount,
            VersionNumber = p.VersionNumber,
            Notes = p.Notes
        };

        var historyList = await _context.Proposals
            .Include(x => x.ProjectScope)
                .ThenInclude(ps => ps!.Project)
                    .ThenInclude(proj => proj!.Client)
            .Include(x => x.ProjectScope)
                .ThenInclude(ps => ps!.ScopeOfWork)
            .Include(x => x.ProposalStatus)
            .Include(x => x.ReferedByUser)
            .Where(x => x.ProposalNumber == p.ProposalNumber)
            .OrderByDescending(x => x.VersionNumber)
            .Select(x => new ProposalResponseDTO
            {
                Id = x.Id,
                ProposalNumber = x.ProposalNumber,
                ProjectScopeId = x.ProjectScopeId,
                ProjectName = x.ProjectScope != null && x.ProjectScope.Project != null ? x.ProjectScope.Project.Name : string.Empty,
                ClientName = x.ProjectScope != null && x.ProjectScope.Project != null && x.ProjectScope.Project.Client != null ? x.ProjectScope.Project.Client.ClientName : string.Empty,
                StatusId = x.StatusId,
                StatusName = x.ProposalStatus != null ? x.ProposalStatus.Name : "Unknown",
                ReferedBy = x.ReferedBy,
                ReferedByUserName = x.ReferedByUser != null ? x.ReferedByUser.UserName ?? "Unknown" : "Unknown",
                Price = x.Price,
                Vat = x.Vat,
                ReceivedFromClient = x.ReceivedFromClient,
                PendingAmount = x.PendingAmount,
                VersionNumber = x.VersionNumber,
                Notes = x.Notes
            })
            .ToListAsync();

        var result = new ProposalDetailsWithHistoryResponseDTO
        {
            Details = detailsDto,
            History = historyList
        };

        return new ApiResponse<ProposalDetailsWithHistoryResponseDTO>(200, result);
    }

    public async Task<ApiResponse<ProposalResponseDTO>> CreateProposalAsync(CreateProposalRequestDTO createProposalDTO)
    {
        var scopeExists = await _context.Projects_Scopes.AnyAsync(s => s.Id == createProposalDTO.ProjectScopeId);
        if (!scopeExists)
        {
            return new ApiResponse<ProposalResponseDTO>(400, "Invalid ProjectScopeId.");
        }

        // Validate Referred By user exists and is an Inspector (RoleId = 2)
        var isInspector = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == createProposalDTO.ReferedBy && ur.RoleId == 2);
        if (!isInspector)
        {
            return new ApiResponse<ProposalResponseDTO>(400, "Referred user must exist and have the Inspector role.");
        }

        // Generate sequential ProposalNumber: AH-YYxxxx
        var currentYearPrefix = $"AH-{DateTime.UtcNow:yy}";
        var lastProposal = await _context.Proposals
            .Where(p => p.ProposalNumber.StartsWith(currentYearPrefix))
            .OrderByDescending(p => p.Id)
            .FirstOrDefaultAsync();

        int nextSeq = 1;
        if (lastProposal != null && lastProposal.ProposalNumber.Length >= 9)
        {
            var suffix = lastProposal.ProposalNumber.Substring(currentYearPrefix.Length);
            if (int.TryParse(suffix, out var lastSeq))
            {
                nextSeq = lastSeq + 1;
            }
        }
        var proposalNumber = $"{currentYearPrefix}{nextSeq:D4}";

        var vat = createProposalDTO.Price * 0.05m;
        var pending = (createProposalDTO.Price + vat) - createProposalDTO.ReceivedFromClient;

        var proposal = new Proposal
        {
            ProposalNumber = proposalNumber,
            ProjectScopeId = createProposalDTO.ProjectScopeId,
            StatusId = 1, // Pending
            ReferedBy = createProposalDTO.ReferedBy,
            Price = createProposalDTO.Price,
            Vat = vat,
            ReceivedFromClient = createProposalDTO.ReceivedFromClient,
            PendingAmount = pending,
            VersionNumber = 1,
            Notes = createProposalDTO.Notes ?? string.Empty
        };

        _context.Proposals.Add(proposal);
        await _context.SaveChangesAsync();

        var detailsResponse = await GetProposalByIdAsync(proposal.Id);
        return new ApiResponse<ProposalResponseDTO>(201, detailsResponse.Data!.Details);
    }

    public async Task<ApiResponse<ProposalResponseDTO>> UpdateProposalAsync(int id, UpdateProposalRequestDTO updateProposalDTO)
    {
        var existing = await _context.Proposals.FindAsync(id);
        if (existing == null)
        {
            return new ApiResponse<ProposalResponseDTO>(404, "Proposal not found.");
        }

        // Validate Referred By user exists and is an Inspector
        var isInspector = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == updateProposalDTO.ReferedBy && ur.RoleId == 2);
        if (!isInspector)
        {
            return new ApiResponse<ProposalResponseDTO>(400, "Referred user must exist and have the Inspector role.");
        }

        var vat = updateProposalDTO.Price * 0.05m;
        var pending = (updateProposalDTO.Price + vat) - updateProposalDTO.ReceivedFromClient;

        // Perform In-Place Update (No new row created)
        existing.ReferedBy = updateProposalDTO.ReferedBy;
        existing.Price = updateProposalDTO.Price;
        existing.Vat = vat;
        existing.ReceivedFromClient = updateProposalDTO.ReceivedFromClient;
        existing.PendingAmount = pending;
        existing.Notes = updateProposalDTO.Notes ?? string.Empty;

        await _context.SaveChangesAsync();

        var detailsResponse = await GetProposalByIdAsync(existing.Id);
        return new ApiResponse<ProposalResponseDTO>(200, detailsResponse.Data!.Details);
    }

    public async Task<ApiResponse<ProposalResponseDTO>> CreateProposalVersionAsync(int id, CreateProposalVersionRequestDTO versionDTO)
    {
        var existing = await _context.Proposals.FindAsync(id);
        if (existing == null)
        {
            return new ApiResponse<ProposalResponseDTO>(404, "Base proposal not found.");
        }

        // Validate Referred By user exists and is an Inspector
        var isInspector = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == versionDTO.ReferedBy && ur.RoleId == 2);
        if (!isInspector)
        {
            return new ApiResponse<ProposalResponseDTO>(400, "Referred user must exist and have the Inspector role.");
        }

        // Get max version number for this Proposal Number
        var maxVersion = await _context.Proposals
            .Where(p => p.ProposalNumber == existing.ProposalNumber)
            .MaxAsync(p => p.VersionNumber);

        var vat = versionDTO.Price * 0.05m;
        var pending = (versionDTO.Price + vat) - versionDTO.ReceivedFromClient;

        // Create new Proposal Version row
        var newVersion = new Proposal
        {
            ProposalNumber = existing.ProposalNumber,
            ProjectScopeId = existing.ProjectScopeId,
            StatusId = 1, // Starts as Pending
            ReferedBy = versionDTO.ReferedBy,
            Price = versionDTO.Price,
            Vat = vat,
            ReceivedFromClient = versionDTO.ReceivedFromClient,
            PendingAmount = pending,
            VersionNumber = maxVersion + 1,
            Notes = versionDTO.Notes ?? string.Empty
        };

        _context.Proposals.Add(newVersion);
        await _context.SaveChangesAsync();

        var detailsResponse = await GetProposalByIdAsync(newVersion.Id);
        return new ApiResponse<ProposalResponseDTO>(201, detailsResponse.Data!.Details);
    }

    public async Task<ApiResponse<ProposalResponseDTO>> ApproveProposalAsync(int id)
    {
        var proposal = await _context.Proposals.FindAsync(id);
        if (proposal == null)
        {
            return new ApiResponse<ProposalResponseDTO>(404, "Proposal not found.");
        }

        // Rule: Only one approved proposal can exist per ProjectScope
        var otherApproved = await _context.Proposals
            .Where(p => p.ProjectScopeId == proposal.ProjectScopeId && p.StatusId == 2 && p.Id != proposal.Id)
            .ToListAsync();

        foreach (var other in otherApproved)
        {
            other.StatusId = 3; // Demote previously approved proposals to Rejected
        }

        proposal.StatusId = 2; // Approve
        await _context.SaveChangesAsync();

        var detailsResponse = await GetProposalByIdAsync(proposal.Id);
        return new ApiResponse<ProposalResponseDTO>(200, detailsResponse.Data!.Details);
    }

    public async Task<ApiResponse<ProposalResponseDTO>> RejectProposalAsync(int id)
    {
        var proposal = await _context.Proposals.FindAsync(id);
        if (proposal == null)
        {
            return new ApiResponse<ProposalResponseDTO>(404, "Proposal not found.");
        }

        proposal.StatusId = 3; // Rejected
        await _context.SaveChangesAsync();

        var detailsResponse = await GetProposalByIdAsync(proposal.Id);
        return new ApiResponse<ProposalResponseDTO>(200, detailsResponse.Data!.Details);
    }
}
