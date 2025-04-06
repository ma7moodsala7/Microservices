using Microsoft.EntityFrameworkCore;
using AuditSinkService.Domain.Entities;

namespace AuditSinkService.Persistence;

public class AuditDbContext : DbContext
{
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLogs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.Action).IsRequired();
            entity.Property(e => e.ServiceName).IsRequired();
            entity.Property(e => e.SerializedPayload).IsRequired();
            entity.Property(e => e.UserId).IsRequired(false);
        });
    }
}
