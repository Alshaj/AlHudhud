using AlHudhud.Data;
using AlHudhud.DTOs.Clients;
using AlHudhud.DTOs.Common;
using AlHudhud.Models;
using BestPriceStore.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AlHudhud.Services.ClientsService;

public class ClientsService : IClientsService
{
    private readonly ApplicationDbContext _context;

    public ClientsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<PaginatedResultDTO<ClientResponseDTO>>> GetAllClientsAsync(PaginationParametersDTO pagination)
    {
        var query = _context.Clients.AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.Id)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(c => new ClientResponseDTO
            {
                Id = c.Id,
                ClientName = c.ClientName,
                TaxNumber = c.TaxNumber,
                Email = c.Email,
                CompanyType = c.CompanyType
            })
            .ToListAsync();

        var result = new PaginatedResultDTO<ClientResponseDTO>
        {
            Items = items,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };

        return new ApiResponse<PaginatedResultDTO<ClientResponseDTO>>(200, result);
    }

    public async Task<ApiResponse<ClientResponseDTO>> GetClientByIdAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return new ApiResponse<ClientResponseDTO>(404, "Client not found.");
        }

        var clientDTO = new ClientResponseDTO
        {
            Id = client.Id,
            ClientName = client.ClientName,
            TaxNumber = client.TaxNumber,
            Email = client.Email,
            CompanyType = client.CompanyType
        };

        return new ApiResponse<ClientResponseDTO>(200, clientDTO);
    }

    public async Task<ApiResponse<CreateClientResponseDTO>> CreateClientAsync(CreateClientRequestDTO createClientDTO)
    {
        var client = new Client
        {
            ClientName = createClientDTO.ClientName,
            TaxNumber = createClientDTO.TaxNumber,
            Email = createClientDTO.Email,
            CompanyType = createClientDTO.CompanyType
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return new ApiResponse<CreateClientResponseDTO>(201, new CreateClientResponseDTO
        {
            Id = client.Id,
            Message = "Client created successfully."
        });
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateClientAsync(int id, UpdateClientRequestDTO updateClientDTO)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, "Client not found.");
        }

        client.ClientName = updateClientDTO.ClientName;
        client.TaxNumber = updateClientDTO.TaxNumber;
        client.Email = updateClientDTO.Email;
        client.CompanyType = updateClientDTO.CompanyType;

        await _context.SaveChangesAsync();

        return new ApiResponse<ConfirmationResponseDTO>(200, new ConfirmationResponseDTO
        {
            Message = "Client updated successfully."
        });
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteClientAsync(int id)
    {
        var client = await _context.Clients
            .Include(c => c.Projects)
            .FirstOrDefaultAsync(c => c.Id == id);
            
        if (client == null)
        {
            return new ApiResponse<ConfirmationResponseDTO>(404, "Client not found.");
        }

        if (client.Projects.Any())
        {
            return new ApiResponse<ConfirmationResponseDTO>(400, "Cannot delete client because they have associated projects.");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return new ApiResponse<ConfirmationResponseDTO>(200, new ConfirmationResponseDTO
        {
            Message = "Client deleted successfully."
        });
    }
}
