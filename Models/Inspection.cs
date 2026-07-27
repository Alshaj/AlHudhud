namespace AlHudhud.Models;

public class Inspection
{
    public int Id { get; set; }
    public int ProposalId { get; set; }
    public int InspectorId { get; set; }
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public string Location { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public int InspectionOrder { get; set; }
    public string Notes { get; set; } = string.Empty;

    public virtual Proposal? Proposal { get; set; }
    public virtual ApplicationUser? Inspector { get; set; }
}
