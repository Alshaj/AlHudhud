using BestPriceStore.DTOs;

namespace AlHudhud.Services.RolesService;

public interface IRolesService
{
    Task<ApiResponse<List<RoleResponseDTO>>> GetAllRolesAsync();
}
