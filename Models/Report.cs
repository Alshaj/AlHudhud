namespace AlHudhud.Models;

public class Report
{
    public int Id { get; set; }
    public string ReportNumber { get; set; } = string.Empty;
    public int ProjectScopeId { get; set; }

    public virtual ProjectScope? ProjectScope { get; set; }
}
