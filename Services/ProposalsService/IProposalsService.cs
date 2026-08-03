using AlHudhud.DTOs.Proposals;
using BestPriceStore.DTOs;

namespace AlHudhud.Services.ProposalsService;

public interface IProposalsService
{
    Task<ApiResponse<List<ProposalResponseDTO>>> GetAllProposalsAsync();
    Task<ApiResponse<ConfirmationResponseDTO>> CreateProposalAsync(CreateProposalRequestDTO request);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateProposalAsync(int id, UpdateProposalRequestDTO request);
    Task<ApiResponse<ConfirmationResponseDTO>> ChangeProposalStatusAsync(int id, ChangeProposalStatusDTO request);
    Task<ApiResponse<ConfirmationResponseDTO>> CreateProposalVersionAsync(int id, CreateProposalVersionRequestDTO request);
    Task<ApiResponse<List<ProposalResponseDTO>>> GetProposalHistoryAsync(int id);
}
