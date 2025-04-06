using AppointmentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.Persistence;

public class AppointmentDbContext : DbContext
{
    public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
    {
    }

    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.LawyerId).IsRequired();
            entity.Property(e => e.ScheduledAt).IsRequired();
            entity.Property(e => e.Status).IsRequired();
        });
    }
}
