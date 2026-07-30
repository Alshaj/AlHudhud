using AlHudhud.Data;
using AlHudhud.Models;
using BestPriceStore.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AlHudhud.Services.ScopesOfWorkService;

public class ScopesOfWorkService : IScopesOfWorkService
{
    private readonly ApplicationDbContext _context;

    public ScopesOfWorkService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<ScopeOfWorkResponseDTO>>> GetAllScopesAsync()
    {
        var scopes = await _context.ScopesOfWork
            .Where(s => !s.IsDeleted)
            .Select(s => new ScopeOfWorkResponseDTO
            {
                Id = s.Id,
                Name = s.Name,
                IsNeedInspection = s.IsNeedInspection
            })
            .ToListAsync();

        if(scopes == null )
        {
            return new ApiResponse<List<ScopeOfWorkResponseDTO>>(404, "No scopes of work found.");
        }

        return new ApiResponse<List<ScopeOfWorkResponseDTO>>(200, scopes);
    }

    public async Task<ApiResponse<ScopeOfWorkResponseDTO>> GetScopeByIdAsync(int id)
    {
        var scope = await _context.ScopesOfWork.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        if (scope == null)
        {
            return new ApiResponse<ScopeOfWorkResponseDTO>(404, "Scope of work not found.");
        }

        var scopeDTO = new ScopeOfWorkResponseDTO
        {
            Id = scope.Id,
            Name = scope.Name,
            IsNeedInspection = scope.IsNeedInspection
        };

        return new ApiResponse<ScopeOfWorkResponseDTO>(200, scopeDTO);
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> CreateScopeAsync(CreateScopeOfWorkRequestDTO createScopeDTO)
    {
        var scope = new ScopeOfWork
        {
            Name = createScopeDTO.Name,
            IsNeedInspection = createScopeDTO.IsNeedInspection
        };

        _context.ScopesOfWork.Add(scope);
        await _context.SaveChangesAsync();

        return new ApiResponse<ConfirmationResponseDTO>(201, new ConfirmationResponseDTO
        {
            Message = "Scope of work created successfully."
        });
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateScopeAsync(int id, UpdateScopeOfWorkRequestDTO updateScopeDTO)
    {
        var scope = await _context.ScopesOfWork.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        if (scope == null)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, new ConfirmationResponseDTO
            {
                Message = "Scope of work not found."
            });
        }

        scope.Name = updateScopeDTO.Name;
        scope.IsNeedInspection = updateScopeDTO.IsNeedInspection;

        await _context.SaveChangesAsync();

        var scopeResponse = new ScopeOfWorkResponseDTO
        {
            Id = scope.Id,
            Name = scope.Name,
            IsNeedInspection = scope.IsNeedInspection
        };

        return new ApiResponse<ConfirmationResponseDTO>(200, new ConfirmationResponseDTO
        {
            Message = "Scope of work updated successfully."
        });
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteScopeAsync(int id)
    {
        var scope = await _context.ScopesOfWork.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        if (scope == null)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, new ConfirmationResponseDTO
            {
                Message = "Scope of work not found."
            });
        }

        // Soft Delete
        scope.IsDeleted = true;
        await _context.SaveChangesAsync();

        return new ApiResponse<ConfirmationResponseDTO>(200, new ConfirmationResponseDTO
        {
            Message = "Scope of work deleted successfully."
        });
    }
}
