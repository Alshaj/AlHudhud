namespace AlHudhud.Models;

public class ProposalStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
}
