namespace AlHudhud.Models;

public class ProjectScope
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int ScopeOfWorkId { get; set; }
    public string Location { get; set; } = string.Empty;
    public int Projects_Scopes_Statuses_Id { get; set; }

    public virtual Project? Project { get; set; }
    public virtual ScopeOfWork? ScopeOfWork { get; set; }
    public virtual ProjectScopeStatus? ProjectScopeStatus { get; set; }

    public virtual ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
    public virtual Report? Report { get; set; }
    public virtual Certificate? Certificate { get; set; }
}
