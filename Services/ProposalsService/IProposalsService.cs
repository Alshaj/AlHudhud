using BestPriceStore.DTOs;

namespace AlHudhud.Services.ProposalsService;

public interface IProposalsService
{
    Task<ApiResponse<List<ProposalResponseDTO>>> GetAllProposalsAsync();
    Task<ApiResponse<ProposalDetailsWithHistoryResponseDTO>> GetProposalByIdAsync(int id);
    Task<ApiResponse<ConfirmationResponseDTO>> CreateProposalAsync(CreateProposalRequestDTO createProposalDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateProposalAsync(int id, UpdateProposalRequestDTO updateProposalDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> CreateProposalVersionAsync(int id, CreateProposalVersionRequestDTO versionDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> ApproveProposalAsync(int id);
    Task<ApiResponse<ConfirmationResponseDTO>> RejectProposalAsync(int id);
}
