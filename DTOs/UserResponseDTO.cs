namespace BestPriceStore.DTOs;

public class UserResponseDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string Role { get; set; } = string.Empty;
    public int RoleId { get; set; }
}
