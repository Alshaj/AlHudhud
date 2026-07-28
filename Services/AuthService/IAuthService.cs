using BestPriceStore.DTOs;

namespace AlHudhud.Services.AuthService;

public interface IAuthService
{
    Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO);
    Task<ApiResponse<LoginResponseDTO>> RefreshTokenAsync();
    Task<ApiResponse<ConfirmationResponseDTO>> LogoutAsync();
    Task<ApiResponse<ConfirmationResponseDTO>> ChangePasswordAsync(ChangePasswordRequestDTO changePasswordDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> ForgotPasswordAsync(ForgotPasswordRequestDTO forgotPasswordDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> ResetPasswordAsync(ResetPasswordRequestDTO resetPasswordDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> ResendOtpAsync(ResendOtpRequestDTO resendOtpDTO);
}
