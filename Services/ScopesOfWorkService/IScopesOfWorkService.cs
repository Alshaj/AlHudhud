using BestPriceStore.DTOs;

namespace AlHudhud.Services.ScopesOfWorkService;

public interface IScopesOfWorkService
{
    Task<ApiResponse<List<ScopeOfWorkResponseDTO>>> GetAllScopesAsync();
    Task<ApiResponse<ScopeOfWorkResponseDTO>> GetScopeByIdAsync(int id);
    Task<ApiResponse<ConfirmationResponseDTO>> CreateScopeAsync(CreateScopeOfWorkRequestDTO createScopeDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateScopeAsync(int id, UpdateScopeOfWorkRequestDTO updateScopeDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> DeleteScopeAsync(int id);
}
