using AlHudhud.DTOs.Clients;
using AlHudhud.DTOs.Common;
using BestPriceStore.DTOs;

namespace AlHudhud.Services.ClientsService;

public interface IClientsService
{
    Task<ApiResponse<PaginatedResultDTO<ClientResponseDTO>>> GetAllClientsAsync(int page = 1, int pageSize = 10, string? search = null);
    Task<ApiResponse<ClientResponseDTO>> GetClientByIdAsync(int id);
    Task<ApiResponse<CreateClientResponseDTO>> CreateClientAsync(CreateClientRequestDTO createClientDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateClientAsync(int id, UpdateClientRequestDTO updateClientDTO);
    Task<ApiResponse<ConfirmationResponseDTO>> DeleteClientAsync(int id);
}
