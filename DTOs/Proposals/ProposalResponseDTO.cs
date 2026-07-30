namespace BestPriceStore.DTOs;

public class ProposalResponseDTO
{
    public int Id { get; set; }
    public string ProposalNumber { get; set; } = string.Empty;
    public int ProjectScopeId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public int ReferedBy { get; set; }
    public string ReferedByUserName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Vat { get; set; }
    public decimal ReceivedFromClient { get; set; }
    public decimal PendingAmount { get; set; }
    public int VersionNumber { get; set; }
    public string Notes { get; set; } = string.Empty;
}
