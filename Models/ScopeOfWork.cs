namespace AlHudhud.Models;

public class ScopeOfWork
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsNeedInspection { get; set; }

    public virtual ICollection<ProjectScope> ProjectScopes { get; set; } = new List<ProjectScope>();
}
