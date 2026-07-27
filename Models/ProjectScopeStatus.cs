namespace AlHudhud.Models;

public class ProjectScopeStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<ProjectScope> ProjectScopes { get; set; } = new List<ProjectScope>();
}
