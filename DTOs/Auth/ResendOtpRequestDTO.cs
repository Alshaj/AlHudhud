using System.ComponentModel.DataAnnotations;

namespace BestPriceStore.DTOs;

public class ResendOtpRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
