namespace AlHudhud.Models;

public class Project
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public virtual Client? Client { get; set; }
    public virtual ICollection<ProjectScope> ProjectScopes { get; set; } = new List<ProjectScope>();
}
