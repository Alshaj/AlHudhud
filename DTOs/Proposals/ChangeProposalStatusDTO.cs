using System.ComponentModel.DataAnnotations;

namespace AlHudhud.DTOs.Proposals;

public class ChangeProposalStatusDTO
{
    [Required]
    [Range(1, 3, ErrorMessage = "Status ID must be 1 (Pending), 2 (Approved), or 3 (Rejected).")]
    public int StatusId { get; set; }
}
