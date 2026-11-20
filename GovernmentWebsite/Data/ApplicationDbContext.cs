using GovernmentWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace GovernmentWebsite.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<FormSubmission> FormSubmissions => Set<FormSubmission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FormSubmission>(entity =>
        {
            entity.ToTable("FormSubmissions");

            entity.HasIndex(e => e.ReferenceCode)
                  .IsUnique();

            entity.HasIndex(e => e.Email);

            entity.HasIndex(e => e.FormType);

            entity.HasIndex(e => e.SubmittedAt);
        });
    }
}
