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

    public async Task<ApiResponse<List<RoleResponseDTO>>> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles
            .Select(r => new RoleResponseDTO
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty
            })
            .ToListAsync();

        if (roles == null)
        {
            return new ApiResponse<List<RoleResponseDTO>>(404, "No Roles Found");
        }

        return new ApiResponse<List<RoleResponseDTO>>(200, roles);
    }
}
