using BestPriceStore.DTOs;

namespace AlHudhud.Services.UsersService;

public interface IUsersService
{
    Task<ApiResponse<IEnumerable<UserResponseDTO>>> GetAllUsersAsync();
    Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int id);
    Task<ApiResponse<UserResponseDTO>> CreateUserAsync(CreateUserRequestDTO createUserDTO);
    Task<ApiResponse<UserResponseDTO>> UpdateUserAsync(int id, UpdateUserRequestDTO updateUserDTO);
    Task<ApiResponse<UserResponseDTO>> ToggleUserStatusAsync(int id, ChangeUserStatusDTO changeStatusDTO);
    Task<ApiResponse<string>> DeleteUserAsync(int id);
}
