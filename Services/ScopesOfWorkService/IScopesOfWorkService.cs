using AlHudhud.DTOs.Common;
using BestPriceStore.DTOs;

namespace AlHudhud.Services.ScopesOfWorkService;

public interface IScopesOfWorkService
{
    Task<ApiResponse<PaginatedResultDTO<ScopeOfWorkResponseDTO>>> GetAllScopesAsync(int page = 1, int pageSize = 10, string? search = null);
    Task<ApiResponse<ScopeOfWorkResponseDTO>> GetScopeByIdAsync(int id);
    Task<ApiResponse<ConfirmationResponseDTO>> CreateScopeAsync(CreateScopeOfWorkRequestDTO createScopeDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateScopeAsync(int id, UpdateScopeOfWorkRequestDTO updateScopeDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> DeleteScopeAsync(int id);
}
