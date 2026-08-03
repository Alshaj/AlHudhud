using AlHudhud.DTOs.Clients;
using AlHudhud.DTOs.Common;
using BestPriceStore.DTOs;

namespace AlHudhud.Services.ClientsService;

public interface IClientsService
{
    Task<ApiResponse<PaginatedResultDTO<ClientResponseDTO>>> GetAllClientsAsync(PaginationParametersDTO pagination);
    Task<ApiResponse<ClientResponseDTO>> GetClientByIdAsync(int id);
    Task<ApiResponse<CreateClientResponseDTO>> CreateClientAsync(CreateClientRequestDTO createClientDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateClientAsync(int id, UpdateClientRequestDTO updateClientDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> DeleteClientAsync(int id);
}
