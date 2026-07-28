using AlHudhud.Services.AuthService;
using BestPriceStore.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AlHudhud.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [EnableRateLimiting("AuthLimiter")]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login([FromBody] LoginRequestDTO loginRequestDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid login data.");
        }
        var response = await _authService.LoginAsync(loginRequestDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [EnableRateLimiting("AuthLimiter")]
    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> RefreshToken()
    {
        var response = await _authService.RefreshTokenAsync();
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var response = await _authService.LogoutAsync();
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> ChangePassword([FromBody] ChangePasswordRequestDTO changePasswordDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid change password data.");
        }
        var response = await _authService.ChangePasswordAsync(changePasswordDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> ForgotPassword([FromBody] ForgotPasswordRequestDTO forgotPasswordDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid forgot password request.");
        }
        var response = await _authService.ForgotPasswordAsync(forgotPasswordDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> ResetPassword([FromBody] ResetPasswordRequestDTO resetPasswordDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid reset password request.");
        }
        var response = await _authService.ResetPasswordAsync(resetPasswordDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [EnableRateLimiting("AuthLimiter")]
    [HttpPost("resend-otp")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> ResendOtp([FromBody] ResendOtpRequestDTO resendOtpDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid resend OTP request.");
        }
        var response = await _authService.ResendOtpAsync(resendOtpDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }
}
