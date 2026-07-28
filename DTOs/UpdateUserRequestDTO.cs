using System.ComponentModel.DataAnnotations;

namespace BestPriceStore.DTOs;

public class UpdateUserRequestDTO
{
    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    [Required]
    public int RoleId { get; set; }
}
