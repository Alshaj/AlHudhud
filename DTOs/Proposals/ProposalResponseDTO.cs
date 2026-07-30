namespace AlHudhud.DTOs.Proposals;

public class ProposalResponseDTO
{
    public int Id { get; set; }
    public string ProposalNumber { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string ReferedBy { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Vat { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
