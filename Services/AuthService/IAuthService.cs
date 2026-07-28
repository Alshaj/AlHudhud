using BestPriceStore.DTOs;

namespace AlHudhud.Services.AuthService;

public interface IAuthService
{
    Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO);
    Task<ApiResponse<LoginResponseDTO>> RefreshTokenAsync();
    Task<ApiResponse<string>> LogoutAsync();
    Task<ApiResponse<string>> ChangePasswordAsync(ChangePasswordRequestDTO changePasswordDTO);
    Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordRequestDTO forgotPasswordDTO);
    Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordRequestDTO resetPasswordDTO);
    Task<ApiResponse<string>> ResendOtpAsync(ResendOtpRequestDTO resendOtpDTO);
}
