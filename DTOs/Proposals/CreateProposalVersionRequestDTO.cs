using System.ComponentModel.DataAnnotations;

namespace BestPriceStore.DTOs;

public class CreateProposalVersionRequestDTO
{
    [Required]
    public int ReferedBy { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Received amount cannot be negative.")]
    public decimal ReceivedFromClient { get; set; }

    public string Notes { get; set; } = string.Empty;
}
