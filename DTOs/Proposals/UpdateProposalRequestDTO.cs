using System.ComponentModel.DataAnnotations;

namespace AlHudhud.DTOs.Proposals;

public class UpdateProposalRequestDTO
{
    [Required]
    public int ClientId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Project name must be between 2 and 100 characters.")]
    public string ProjectName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 100 characters.")]
    public string Location { get; set; } = string.Empty;

    [Required]
    public int ScopeOfWorkId { get; set; }

    [Required]
    public int ReferedById { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    public string Notes { get; set; } = string.Empty;
}
