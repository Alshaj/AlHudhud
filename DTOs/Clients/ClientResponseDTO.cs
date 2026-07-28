namespace BestPriceStore.DTOs;

public class ClientResponseDTO
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string? TaxNumber { get; set; }
    public string? Email { get; set; }
    public string? CompanyType { get; set; }
}
