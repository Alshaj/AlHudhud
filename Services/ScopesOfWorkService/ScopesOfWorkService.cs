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

    public async Task<ApiResponse<IEnumerable<ScopeOfWorkResponseDTO>>> GetAllScopesAsync()
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

        return new ApiResponse<IEnumerable<ScopeOfWorkResponseDTO>>(200, scopes);
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

    public async Task<ApiResponse<ScopeOfWorkResponseDTO>> CreateScopeAsync(CreateScopeOfWorkRequestDTO createScopeDTO)
    {
        var scope = new ScopeOfWork
        {
            Name = createScopeDTO.Name,
            IsNeedInspection = createScopeDTO.IsNeedInspection
        };

        _context.ScopesOfWork.Add(scope);
        await _context.SaveChangesAsync();

        var scopeResponse = new ScopeOfWorkResponseDTO
        {
            Id = scope.Id,
            Name = scope.Name,
            IsNeedInspection = scope.IsNeedInspection
        };

        return new ApiResponse<ScopeOfWorkResponseDTO>(201, scopeResponse);
    }

    public async Task<ApiResponse<ScopeOfWorkResponseDTO>> UpdateScopeAsync(int id, UpdateScopeOfWorkRequestDTO updateScopeDTO)
    {
        var scope = await _context.ScopesOfWork.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        if (scope == null)
        {
            return new ApiResponse<ScopeOfWorkResponseDTO>(404, "Scope of work not found.");
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

        return new ApiResponse<ScopeOfWorkResponseDTO>(200, scopeResponse);
    }

    public async Task<ApiResponse<string>> DeleteScopeAsync(int id)
    {
        var scope = await _context.ScopesOfWork.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        if (scope == null)
        {
            return new ApiResponse<string>(404, "Scope of work not found.");
        }

        // Soft Delete
        scope.IsDeleted = true;
        await _context.SaveChangesAsync();

        return new ApiResponse<string>(200, "Scope of work deleted successfully.");
    }
}
