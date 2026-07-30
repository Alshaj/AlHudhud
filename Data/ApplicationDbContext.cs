using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AlHudhud.Models;

namespace AlHudhud.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ScopeOfWork> ScopesOfWork { get; set; }
    public DbSet<ProjectScope> Projects_Scopes { get; set; }
    public DbSet<ProjectScopeStatus> Projects_Scopes_Statuses { get; set; }
    public DbSet<ProposalStatus> Proposal_Statuses { get; set; }
    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<Inspection> Inspections { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Rename Identity Tables (optional, but standard for consistency)
        builder.Entity<ApplicationUser>().ToTable("Identity_Users");
        builder.Entity<ApplicationRole>().ToTable("Roles");

        // Unique Constraint for Projects_Scopes
        builder.Entity<ProjectScope>()
            .HasIndex(ps => new { ps.ProjectId, ps.ScopeOfWorkId })
            .IsUnique();
        
        // Define relations with ReferedByUser in Proposals
        builder.Entity<Proposal>()
            .HasOne(p => p.ReferedByUser)
            .WithMany(u => u.ProposalsReferred)
            .HasForeignKey(p => p.ReferedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Define relations with ProposalStatus in Proposals
        builder.Entity<Proposal>()
            .HasOne(p => p.ProposalStatus)
            .WithMany(s => s.Proposals)
            .HasForeignKey(p => p.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Proposal>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Entity<Proposal>()
            .Property(p => p.TotalAmount)
            .HasDefaultValue(0m);

        // Define relations with Inspector in Inspections
        builder.Entity<Inspection>()
            .HasOne(i => i.Inspector)
            .WithMany(u => u.Inspections)
            .HasForeignKey(i => i.InspectorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Roles
        builder.Entity<ApplicationRole>().HasData(
            new ApplicationRole { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
            new ApplicationRole { Id = 2, Name = "Inspector", NormalizedName = "INSPECTOR" },
            new ApplicationRole { Id = 3, Name = "Viewer", NormalizedName = "VIEWER" }
        );

        // Seed ProjectScopeStatus
        builder.Entity<ProjectScopeStatus>().HasData(
            new ProjectScopeStatus { Id = 1, Name = "Inprogress" },
            new ProjectScopeStatus { Id = 2, Name = "Completed" }
        );

        // Seed ProposalStatus
        builder.Entity<ProposalStatus>().HasData(
            new ProposalStatus { Id = 1, Name = "Pending" },
            new ProposalStatus { Id = 2, Name = "Approved" },
            new ProposalStatus { Id = 3, Name = "Rejected" }
        );
    }
}
