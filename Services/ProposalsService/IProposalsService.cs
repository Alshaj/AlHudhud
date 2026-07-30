using AlHudhud.DTOs.Proposals;
using BestPriceStore.DTOs;

namespace AlHudhud.Services.ProposalsService;

public interface IProposalsService
{
    Task<ApiResponse<List<ProposalResponseDTO>>> GetAllProposalsAsync();
    Task<ApiResponse<ConfirmationResponseDTO>> CreateProposalAsync(CreateProposalRequestDTO request);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateProposalAsync(int id, UpdateProposalRequestDTO request);
}
