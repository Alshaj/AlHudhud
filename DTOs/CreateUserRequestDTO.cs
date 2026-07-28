using System.ComponentModel.DataAnnotations;

namespace BestPriceStore.DTOs;

public class CreateUserRequestDTO
{
    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public int RoleId { get; set; }
}
