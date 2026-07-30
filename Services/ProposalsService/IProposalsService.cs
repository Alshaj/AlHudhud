using BestPriceStore.DTOs;

namespace AlHudhud.Services.ProposalsService;

public interface IProposalsService
{
    Task<ApiResponse<IEnumerable<ProposalResponseDTO>>> GetAllProposalsAsync();
    Task<ApiResponse<ProposalDetailsWithHistoryResponseDTO>> GetProposalByIdAsync(int id);
    Task<ApiResponse<ProposalResponseDTO>> CreateProposalAsync(CreateProposalRequestDTO createProposalDTO);
    Task<ApiResponse<ProposalResponseDTO>> UpdateProposalAsync(int id, UpdateProposalRequestDTO updateProposalDTO);
    Task<ApiResponse<ProposalResponseDTO>> CreateProposalVersionAsync(int id, CreateProposalVersionRequestDTO versionDTO);
    Task<ApiResponse<ProposalResponseDTO>> ApproveProposalAsync(int id);
    Task<ApiResponse<ProposalResponseDTO>> RejectProposalAsync(int id);
}
