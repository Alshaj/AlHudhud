using System.ComponentModel.DataAnnotations;

namespace BestPriceStore.DTOs;

public class CreateClientRequestDTO
{
    [Required]
    [StringLength(100)]
    public string ClientName { get; set; } = string.Empty;

    [StringLength(50)]
    public string? TaxNumber { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? CompanyType { get; set; }
}
