namespace AlHudhud.Models;

public class Client
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CompanyType { get; set; } = string.Empty;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
