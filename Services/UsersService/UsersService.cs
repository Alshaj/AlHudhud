using AlHudhud.Models;
using BestPriceStore.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AlHudhud.Services.UsersService;

public class UsersService : IUsersService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UsersService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ApiResponse<IEnumerable<UserResponseDTO>>> GetAllUsersAsync()
    {
        var usersList = await _userManager.Users.ToListAsync();
        var usersResponse = new List<UserResponseDTO>();

        foreach (var user in usersList)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? "No Role";
            var role = await _roleManager.FindByNameAsync(roleName);
            var roleId = role?.Id ?? 0;

            usersResponse.Add(new UserResponseDTO
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.Is_Active,
                Role = roleName,
                RoleId = roleId
            });
        }

        return new ApiResponse<IEnumerable<UserResponseDTO>>(200, usersResponse);
    }

    public async Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return new ApiResponse<UserResponseDTO>(404, "User not found.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var roleName = roles.FirstOrDefault() ?? "No Role";
        var role = await _roleManager.FindByNameAsync(roleName);
        var roleId = role?.Id ?? 0;

        var userDTO = new UserResponseDTO
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.Is_Active,
            Role = roleName,
            RoleId = roleId
        };

        return new ApiResponse<UserResponseDTO>(200, userDTO);
    }

    public async Task<ApiResponse<UserResponseDTO>> CreateUserAsync(CreateUserRequestDTO createUserDTO)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == createUserDTO.RoleId);
        if (role == null)
        {
            return new ApiResponse<UserResponseDTO>(400, "Invalid Role ID.");
        }

        var user = new ApplicationUser
        {
            UserName = createUserDTO.UserName,
            Email = createUserDTO.Email,
            PhoneNumber = createUserDTO.PhoneNumber,
            Is_Active = true // Active by default on creation
        };

        var result = await _userManager.CreateAsync(user, createUserDTO.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return new ApiResponse<UserResponseDTO>(400, errors);
        }

        await _userManager.AddToRoleAsync(user, role.Name!);

        var userResponse = new UserResponseDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.Is_Active,
            Role = role.Name!,
            RoleId = role.Id
        };

        return new ApiResponse<UserResponseDTO>(201, userResponse);
    }

    public async Task<ApiResponse<UserResponseDTO>> UpdateUserAsync(int id, UpdateUserRequestDTO updateUserDTO)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return new ApiResponse<UserResponseDTO>(404, "User not found.");
        }

        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == updateUserDTO.RoleId);
        if (role == null)
        {
            return new ApiResponse<UserResponseDTO>(400, "Invalid Role ID.");
        }

        user.UserName = updateUserDTO.UserName;
        user.Email = updateUserDTO.Email;
        user.PhoneNumber = updateUserDTO.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return new ApiResponse<UserResponseDTO>(400, errors);
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (!currentRoles.Contains(role.Name!))
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role.Name!);
        }

        var userResponse = new UserResponseDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.Is_Active,
            Role = role.Name!,
            RoleId = role.Id
        };

        return new ApiResponse<UserResponseDTO>(200, userResponse);
    }

    public async Task<ApiResponse<UserResponseDTO>> ToggleUserStatusAsync(int id, ChangeUserStatusDTO changeStatusDTO)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return new ApiResponse<UserResponseDTO>(404, "User not found.");
        }

        user.Is_Active = changeStatusDTO.IsActive;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return new ApiResponse<UserResponseDTO>(400, errors);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var roleName = roles.FirstOrDefault() ?? "No Role";
        var role = await _roleManager.FindByNameAsync(roleName);
        var roleId = role?.Id ?? 0;

        var userResponse = new UserResponseDTO
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.Is_Active,
            Role = roleName,
            RoleId = roleId
        };

        return new ApiResponse<UserResponseDTO>(200, userResponse);
    }

    public async Task<ApiResponse<string>> DeleteUserAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return new ApiResponse<string>(404, "User not found.");
        }

        // Soft Delete: Disable account to block future logins
        user.Is_Active = false;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return new ApiResponse<string>(400, errors);
        }

        return new ApiResponse<string>(200, "User deleted successfully.");
    }
}
