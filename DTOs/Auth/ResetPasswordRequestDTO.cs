using System.ComponentModel.DataAnnotations;

namespace BestPriceStore.DTOs;

public class ResetPasswordRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be exactly 6 digits.")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must contain only digits.")]
    public string Otp { get; set; } = string.Empty;

    [Required]
    public string NewPassword { get; set; } = string.Empty;
}
