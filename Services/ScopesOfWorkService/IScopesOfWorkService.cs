using BestPriceStore.DTOs;

namespace AlHudhud.Services.ScopesOfWorkService;

public interface IScopesOfWorkService
{
    Task<ApiResponse<IEnumerable<ScopeOfWorkResponseDTO>>> GetAllScopesAsync();
    Task<ApiResponse<ScopeOfWorkResponseDTO>> GetScopeByIdAsync(int id);
    Task<ApiResponse<ScopeOfWorkResponseDTO>> CreateScopeAsync(CreateScopeOfWorkRequestDTO createScopeDTO);
    Task<ApiResponse<ScopeOfWorkResponseDTO>> UpdateScopeAsync(int id, UpdateScopeOfWorkRequestDTO updateScopeDTO);
    Task<ApiResponse<string>> DeleteScopeAsync(int id);
}
