using System.ComponentModel.DataAnnotations;

namespace BestPriceStore.DTOs;

public class CreateProposalRequestDTO
{
    [Required]
    public int ClientId { get; set; }

    [Required]
    [StringLength(200)]
    public string ProjectName { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Location { get; set; } = string.Empty;

    [Required]
    public int ScopeOfWorkId { get; set; }

    [Required]
    public int ReferedBy { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Received amount cannot be negative.")]
    public decimal? ReceivedFromClient { get; set; }

    public string Notes { get; set; } = string.Empty;
}
