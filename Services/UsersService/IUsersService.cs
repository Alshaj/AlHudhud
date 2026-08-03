using AlHudhud.DTOs.Common;
using BestPriceStore.DTOs;

namespace AlHudhud.Services.UsersService;

public interface IUsersService
{
    Task<ApiResponse<PaginatedResultDTO<UserResponseDTO>>> GetAllUsersAsync(PaginationParametersDTO pagination);
    Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int id);
    Task<ApiResponse<ConfirmationResponseDTO>> CreateUserAsync(CreateUserRequestDTO createUserDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateUserAsync(int id, UpdateUserRequestDTO updateUserDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> ToggleUserStatusAsync(int id, ChangeUserStatusDTO changeStatusDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> DeleteUserAsync(int id);
}
