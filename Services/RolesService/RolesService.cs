using AlHudhud.Models;
using BestPriceStore.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AlHudhud.Services.RolesService;

public class RolesService : IRolesService
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RolesService(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<ApiResponse<IEnumerable<RoleResponseDTO>>> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles
            .Select(r => new RoleResponseDTO
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty
            })
            .ToListAsync();

        return new ApiResponse<IEnumerable<RoleResponseDTO>>(200, roles);
    }
}
