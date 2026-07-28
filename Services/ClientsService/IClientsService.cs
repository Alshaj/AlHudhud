using BestPriceStore.DTOs;

namespace AlHudhud.Services.ClientsService;

public interface IClientsService
{
    Task<ApiResponse<IEnumerable<ClientResponseDTO>>> GetAllClientsAsync();
    Task<ApiResponse<ClientResponseDTO>> GetClientByIdAsync(int id);
    Task<ApiResponse<ClientResponseDTO>> CreateClientAsync(CreateClientRequestDTO createClientDTO);
    Task<ApiResponse<ClientResponseDTO>> UpdateClientAsync(int id, UpdateClientRequestDTO updateClientDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> DeleteClientAsync(int id);
}
