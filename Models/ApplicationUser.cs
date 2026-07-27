using Microsoft.AspNetCore.Identity;

namespace AlHudhud.Models;

public class ApplicationUser : IdentityUser<int>
{
    public bool Is_Active { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string? PasswordResetOtp { get; set; }
    public DateTime? PasswordResetOtpExpiryTime { get; set; }

    // Navigation properties for relationships
    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    public virtual ICollection<Proposal> ProposalsReferred { get; set; } = new List<Proposal>();
}
