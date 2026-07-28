using BestPriceStore.DTOs;

namespace AlHudhud.Services.RolesService;

public interface IRolesService
{
    Task<ApiResponse<IEnumerable<RoleResponseDTO>>> GetAllRolesAsync();
}
