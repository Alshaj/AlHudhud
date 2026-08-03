using System.ComponentModel.DataAnnotations;

namespace AlHudhud.DTOs.Proposals;

public class CreateProposalVersionRequestDTO
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    public string? Notes { get; set; }
}
