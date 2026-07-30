namespace BestPriceStore.DTOs;

public class ProposalDetailsWithHistoryResponseDTO
{
    public ProposalResponseDTO Details { get; set; } = null!;
    public IEnumerable<ProposalResponseDTO> History { get; set; } = new List<ProposalResponseDTO>();
}
