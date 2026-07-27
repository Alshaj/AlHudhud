namespace AlHudhud.Models;

public class Certificate
{
    public int Id { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public int ProjectScopeId { get; set; }

    public virtual ProjectScope? ProjectScope { get; set; }
}
