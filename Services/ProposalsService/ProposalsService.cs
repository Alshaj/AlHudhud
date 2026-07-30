using AlHudhud.Data;
using AlHudhud.Models;
using AlHudhud.DTOs.Proposals;
using BestPriceStore.DTOs;
using Microsoft.EntityFrameworkCore;
using AlHudhud.Enums;

namespace AlHudhud.Services.ProposalsService;

public class ProposalsService : IProposalsService
{
    private readonly ApplicationDbContext _context;

    public ProposalsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<ProposalResponseDTO>>> GetAllProposalsAsync()
    {
        var proposals = await _context.Proposals
            .Include(p => p.ProjectScope)
                .ThenInclude(ps => ps!.Project)
                    .ThenInclude(proj => proj!.Client)
            .Include(p => p.ProjectScope)
                .ThenInclude(ps => ps!.ScopeOfWork)
            .Include(p => p.ProposalStatus)
            .Include(p => p.ReferedByUser)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProposalResponseDTO
            {
                Id = p.Id,
                ProposalNumber = p.ProposalNumber,
                ClientName = p.ProjectScope != null && p.ProjectScope.Project != null && p.ProjectScope.Project.Client != null
                    ? p.ProjectScope.Project.Client.ClientName
                    : string.Empty,
                ProjectName = p.ProjectScope != null && p.ProjectScope.Project != null
                    ? p.ProjectScope.Project.Name
                    : string.Empty,
                ScopeOfWork = p.ProjectScope != null && p.ProjectScope.ScopeOfWork != null
                    ? p.ProjectScope.ScopeOfWork.Name
                    : string.Empty,
                ReferedBy = p.ReferedByUser != null
                    ? p.ReferedByUser.UserName ?? string.Empty
                    : string.Empty,
                Price = p.Price,
                Vat = p.Vat,
                TotalAmount = p.TotalAmount,
                CreatedAt = p.CreatedAt,
                Status = p.ProposalStatus != null
                    ? p.ProposalStatus.Name
                    : "Pending"
            })
            .ToListAsync();

        return new ApiResponse<List<ProposalResponseDTO>>(200, proposals);
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> CreateProposalAsync(CreateProposalRequestDTO request)
    {
        // 1. Validate Client
        var clientExists = await _context.Clients.AnyAsync(c => c.Id == request.ClientId);
        if (!clientExists)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, "Client not found.");
        }

        // 2. Validate Scope of Work
        var scopeExists = await _context.ScopesOfWork.AnyAsync(s => s.Id == request.ScopeOfWorkId);
        if (!scopeExists)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, "Scope of work not found.");
        }

        // 3. Validate Referred By user
        var userExists = await _context.Users.AnyAsync(u => u.Id == request.ReferedById);
        if (!userExists)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, "Referred by user not found.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 4. Create Project
            var project = new Project
            {
                ClientId = request.ClientId,
                Name = request.ProjectName,
                Location = request.Location
            };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // 5. Create Project Scope
            var projectScope = new ProjectScope
            {
                ProjectId = project.Id,
                ScopeOfWorkId = request.ScopeOfWorkId,
                Location = request.Location,
                Projects_Scopes_Statuses_Id = 1 // Inprogress
            };
            _context.Projects_Scopes.Add(projectScope);
            await _context.SaveChangesAsync();

            // 6. Generate sequential ProposalNumber
            var yearSuffix = (DateTime.UtcNow.Year % 100).ToString("D2"); // e.g. "26"
            var yearPrefix = $"AH-{yearSuffix}";
            
            var matchingNumbers = await _context.Proposals
                .Where(p => p.ProposalNumber.StartsWith(yearPrefix))
                .Select(p => p.ProposalNumber)
                .ToListAsync();

            int maxSuffix = 0;
            foreach (var num in matchingNumbers)
            {
                if (num.Length > yearPrefix.Length)
                {
                    var suffixStr = num.Substring(yearPrefix.Length);
                    if (int.TryParse(suffixStr, out int val))
                    {
                        if (val > maxSuffix)
                        {
                            maxSuffix = val;
                        }
                    }
                }
            }

            int nextSuffix = maxSuffix + 1;
            var proposalNumber = $"{yearPrefix}{nextSuffix:D4}"; // e.g. "AH-260001" or "AH-260714"

            // 7. Calculate VAT and TotalAmount
            var vat = request.Price * 0.05m;
            var totalAmount = request.Price + vat;

            // 8. Create Proposal
            var proposal = new Proposal
            {
                ProposalNumber = proposalNumber,
                ProjectScopeId = projectScope.Id,
                StatusId = (int)ProposalStatusEnum.Pending,
                ReferedBy = request.ReferedById,
                Price = request.Price,
                Vat = vat,
                TotalAmount = totalAmount,
                VersionNumber = 1,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.Proposals.Add(proposal);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new ApiResponse<ConfirmationResponseDTO>(201, new ConfirmationResponseDTO
            {
                Message = $"Proposal {proposalNumber} created successfully."
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateProposalAsync(int id, UpdateProposalRequestDTO request)
    {
        // 1. Retrieve the existing proposal
        var existingProposal = await _context.Proposals
            .Include(p => p.ProjectScope)
                .ThenInclude(ps => ps!.Project)
            .Include(p => p.ProjectScope)
                .ThenInclude(ps => ps!.Certificate)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (existingProposal == null)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, "Proposal not found.");
        }

        // 2. Validate ProjectScope is not closed or completed
        var projectScope = existingProposal.ProjectScope;
        if (projectScope == null)
        {
            return new ApiResponse<ConfirmationResponseDTO>(400, "Proposal is not associated with a valid project scope.");
        }

        if (projectScope.Projects_Scopes_Statuses_Id == 2 || projectScope.Certificate != null)
        {
            return new ApiResponse<ConfirmationResponseDTO>(400, "Cannot edit proposal because the project scope is closed or completed.");
        }

        // 3. Validate Client exists
        var clientExists = await _context.Clients.AnyAsync(c => c.Id == request.ClientId);
        if (!clientExists)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, "Client not found.");
        }

        // 4. Validate Scope of Work exists
        var scopeExists = await _context.ScopesOfWork.AnyAsync(s => s.Id == request.ScopeOfWorkId);
        if (!scopeExists)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, "Scope of work not found.");
        }

        // 5. Validate Referred By user exists
        var userExists = await _context.Users.AnyAsync(u => u.Id == request.ReferedById);
        if (!userExists)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, "Referred by user not found.");
        }

        // 6. Check unique constraint on ProjectScope (ProjectId, ScopeOfWorkId)
        var scopeConflict = await _context.Projects_Scopes
            .AnyAsync(ps => ps.ProjectId == projectScope.ProjectId && ps.ScopeOfWorkId == request.ScopeOfWorkId && ps.Id != projectScope.Id);
        if (scopeConflict)
        {
            return new ApiResponse<ConfirmationResponseDTO>(400, "This scope of work is already assigned to the project.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 7. Update Project
            var project = projectScope.Project;
            if (project != null)
            {
                project.ClientId = request.ClientId;
                project.Name = request.ProjectName;
                project.Location = request.Location;
            }

            // 8. Update ProjectScope
            projectScope.ScopeOfWorkId = request.ScopeOfWorkId;
            projectScope.Location = request.Location;

            await _context.SaveChangesAsync();

            // 9. Compute VAT and TotalAmount
            var vat = request.Price * 0.05m;
            var totalAmount = request.Price + vat;

            // 10. Update existing Proposal directly (in-place)
            existingProposal.Price = request.Price;
            existingProposal.Vat = vat;
            existingProposal.TotalAmount = totalAmount;
            existingProposal.ReferedBy = request.ReferedById;
            existingProposal.Notes = request.Notes;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new ApiResponse<ConfirmationResponseDTO>(200, new ConfirmationResponseDTO
            {
                Message = $"Proposal {existingProposal.ProposalNumber} updated successfully."
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> ChangeProposalStatusAsync(int id, ChangeProposalStatusDTO request)
    {
        // 1. Retrieve the existing proposal
        var existingProposal = await _context.Proposals.FindAsync(id);
        if (existingProposal == null)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, "Proposal not found.");
        }

        
        // 2. If status is Approved (ID 2), check if another proposal for the same ProjectScope is already approved
        if (request.StatusId == (int)ProposalStatusEnum.Approved)
        {
            var otherApprovedExists = await _context.Proposals
                .AnyAsync(p => p.ProjectScopeId == existingProposal.ProjectScopeId && p.StatusId == (int)ProposalStatusEnum.Approved && p.Id != id);

            if (otherApprovedExists)
            {
                return new ApiResponse<ConfirmationResponseDTO>(400, "Cannot approve this proposal because another proposal is already approved for this project scope.");
            }
        }

        // 3. Update status directly on the proposal row
        existingProposal.StatusId = request.StatusId;
        await _context.SaveChangesAsync();

        string statusName = request.StatusId switch
        {
            (int)ProposalStatusEnum.Pending => "Pending",
            (int)ProposalStatusEnum.Approved => "Approved",
            (int)ProposalStatusEnum.Rejected => "Rejected",
            _ => "Unknown"
        };

        return new ApiResponse<ConfirmationResponseDTO>(200, new ConfirmationResponseDTO
        {
            Message = $"Proposal status updated to {statusName} successfully."
        });
    }
}
