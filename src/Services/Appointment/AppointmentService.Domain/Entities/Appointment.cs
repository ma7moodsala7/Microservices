namespace AppointmentService.Domain.Entities;

public class Appointment
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid LawyerId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
