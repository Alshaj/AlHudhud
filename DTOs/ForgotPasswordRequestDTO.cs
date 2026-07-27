using System.ComponentModel.DataAnnotations;

namespace BestPriceStore.DTOs;

public class ForgotPasswordRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
