namespace BestPriceStore.DTOs;

public class LoginResponseDTO
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>();
}
