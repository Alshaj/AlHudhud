using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AlHudhud.Models;
using AlHudhud.Services.EmailService;
using BestPriceStore.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AlHudhud.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        IEmailService emailService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _emailService = emailService;
    }

    public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password))
        {
            return new ApiResponse<LoginResponseDTO>(404, "Invalid email or password.");
        }

        if (!user.Is_Active)
        {
            return new ApiResponse<LoginResponseDTO>(403, "User account is inactive.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = GenerateJwtToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user);

        InjectTokensIntoCookies(accessToken, refreshToken);

        var loginResponse = new LoginResponseDTO
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            Roles = roles.ToList()
        };

        return new ApiResponse<LoginResponseDTO>(200, loginResponse);
    }

    public async Task<ApiResponse<LoginResponseDTO>> RefreshTokenAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return new ApiResponse<LoginResponseDTO>(401, "Unauthorized.");
        }

        var refreshToken = httpContext.Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return new ApiResponse<LoginResponseDTO>(401, "Refresh token is missing.");
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new ApiResponse<LoginResponseDTO>(401, "Invalid or expired refresh token.");
        }

        if (!user.Is_Active)
        {
            return new ApiResponse<LoginResponseDTO>(403, "User account is inactive.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var newAccessToken = GenerateJwtToken(user);

        InjectAccessTokenCookie(newAccessToken);

        var loginResponse = new LoginResponseDTO
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            Roles = roles.ToList()
        };

        return new ApiResponse<LoginResponseDTO>(200, loginResponse);
    }

    public async Task<ApiResponse<string>> LogoutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var refreshToken = httpContext.Request.Cookies["refresh_token"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = null;
                    await _userManager.UpdateAsync(user);
                }
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                //Domain = ".masarak.app"
            };

            httpContext.Response.Cookies.Delete("access_token", cookieOptions);
            httpContext.Response.Cookies.Delete("refresh_token", cookieOptions);
        }

        return new ApiResponse<string>(200, "Logged out successfully.");
    }

    public async Task<ApiResponse<string>> ChangePasswordAsync(ChangePasswordRequestDTO changePasswordDTO)
    {
        var user = await _userManager.FindByEmailAsync(changePasswordDTO.Email);
        if (user == null)
        {
            return new ApiResponse<string>(404, "User not found.");
        }

        var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return new ApiResponse<string>(400, errors);
        }

        return new ApiResponse<string>(200, "Password changed successfully.");
    }

    public async Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordRequestDTO forgotPasswordDTO)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordDTO.Email);
        if (user == null)
        {
            return new ApiResponse<string>(200, "If the email is registered, we have sent a 6-digit OTP.");
        }

        var otp = new Random().Next(100000, 999999).ToString();
        user.PasswordResetOtp = otp;
        user.PasswordResetOtpExpiryTime = DateTime.UtcNow.AddMinutes(5);

        await _userManager.UpdateAsync(user);

        var emailSubject = "Al Hudhud System - Reset Password OTP";
        var emailBody = $@"
            <h3>Reset Password Request</h3>
            <p>You requested a password reset. Please use the following 6-digit OTP to reset your password:</p>
            <h2 style='color: #007bff; letter-spacing: 2px;'>{otp}</h2>
            <p>This code is valid for <strong>5 minutes</strong>. If you did not request this, please ignore this email.</p>
        ";

        try
        {
            await _emailService.SendEmailAsync(user.Email!, emailSubject, emailBody);
        }
        catch (Exception)
        {
            return new ApiResponse<string>(500, "Failed to send reset email. Please try again later.");
        }

        return new ApiResponse<string>(200, "If the email is registered, we have sent a 6-digit OTP.");
    }

    public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordRequestDTO resetPasswordDTO)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
        if (user == null)
        {
            return new ApiResponse<string>(404, "User not found.");
        }

        if (string.IsNullOrEmpty(user.PasswordResetOtp) || 
            user.PasswordResetOtp != resetPasswordDTO.Otp || 
            user.PasswordResetOtpExpiryTime <= DateTime.UtcNow)
        {
            return new ApiResponse<string>(400, "Invalid or expired OTP.");
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordDTO.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return new ApiResponse<string>(400, errors);
        }

        user.PasswordResetOtp = null;
        user.PasswordResetOtpExpiryTime = null;
        await _userManager.UpdateAsync(user);

        return new ApiResponse<string>(200, "Password has been reset successfully.");
    }

    public async Task<ApiResponse<string>> ResendOtpAsync(ResendOtpRequestDTO resendOtpDTO)
    {
        var user = await _userManager.FindByEmailAsync(resendOtpDTO.Email);
        if (user == null)
        {
            return new ApiResponse<string>(200, "If the email is registered, a new OTP has been sent.");
        }

        var otp = new Random().Next(100000, 999999).ToString();
        user.PasswordResetOtp = otp;
        user.PasswordResetOtpExpiryTime = DateTime.UtcNow.AddMinutes(5);

        await _userManager.UpdateAsync(user);

        var emailSubject = "Al Hudhud System - Resend Password Reset OTP";
        var emailBody = $@"
            <h3>New Reset Password Request</h3>
            <p>You requested a new OTP. Please use the following 6-digit OTP to reset your password:</p>
            <h2 style='color: #007bff; letter-spacing: 2px;'>{otp}</h2>
            <p>This code is valid for <strong>5 minutes</strong>. If you did not request this, please ignore this email.</p>
        ";

        try
        {
            await _emailService.SendEmailAsync(user.Email!, emailSubject, emailBody);
        }
        catch (Exception)
        {
            return new ApiResponse<string>(500, "Failed to send reset email. Please try again later.");
        }

        return new ApiResponse<string>(200, "If the email is registered, a new OTP has been sent.");
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        var roles = _userManager.GetRolesAsync(user).Result;
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<string> GenerateRefreshTokenAsync(ApplicationUser user)
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(3);

        await _userManager.UpdateAsync(user);

        return refreshToken;
    }

    private void InjectTokensIntoCookies(string accessToken, string refreshToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        var accessCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(1),
            Path = "/",
            //Domain = ".masarak.app"
        };

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(3),
            Path = "/",
            //Domain = ".masarak.app"
        };

        httpContext.Response.Cookies.Append("access_token", accessToken, accessCookieOptions);
        httpContext.Response.Cookies.Append("refresh_token", refreshToken, refreshCookieOptions);
    }

    private void InjectAccessTokenCookie(string accessToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        var accessCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(1),
            Path = "/",
            //Domain = ".masarak.app"
        };

        httpContext.Response.Cookies.Append("access_token", accessToken, accessCookieOptions);
    }
}
