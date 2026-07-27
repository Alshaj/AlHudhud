namespace AlHudhud.Models;

public class Proposal
{
    public int Id { get; set; }
    public string ProposalNumber { get; set; } = string.Empty;
    public int ProjectScopeId { get; set; }
    public int StatusId { get; set; }
    public int ReferedBy { get; set; }
    public decimal Price { get; set; }
    public decimal Vat { get; set; }
    public int VersionNumber { get; set; }
    public string Notes { get; set; } = string.Empty;

    public virtual ProjectScope? ProjectScope { get; set; }
    public virtual ProposalStatus? ProposalStatus { get; set; }
    public virtual ApplicationUser? ReferedByUser { get; set; }

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
}
