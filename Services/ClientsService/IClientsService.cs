using AlHudhud.DTOs.Clients;
using BestPriceStore.DTOs;

namespace AlHudhud.Services.ClientsService;

public interface IClientsService
{
    Task<ApiResponse<List<ClientResponseDTO>>> GetAllClientsAsync();
    Task<ApiResponse<ClientResponseDTO>> GetClientByIdAsync(int id);
    Task<ApiResponse<CreateClientResponseDTO>> CreateClientAsync(CreateClientRequestDTO createClientDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateClientAsync(int id, UpdateClientRequestDTO updateClientDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> DeleteClientAsync(int id);
}
