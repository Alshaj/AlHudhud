using AlHudhud.Data;
using AlHudhud.DTOs.Common;
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

    public async Task<ApiResponse<PaginatedResultDTO<ScopeOfWorkResponseDTO>>> GetAllScopesAsync(int page = 1, int pageSize = 10, string? search = null)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : (pageSize > 100 ? 100 : pageSize);

        var query = _context.ScopesOfWork.Where(s => !s.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.Trim().ToLower();
            query = query.Where(s => s.Name != null && s.Name.ToLower().Contains(searchLower));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new ScopeOfWorkResponseDTO
            {
                Id = s.Id,
                Name = s.Name,
                IsNeedInspection = s.IsNeedInspection
            })
            .ToListAsync();

        var result = new PaginatedResultDTO<ScopeOfWorkResponseDTO>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return new ApiResponse<PaginatedResultDTO<ScopeOfWorkResponseDTO>>(200, result);
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
