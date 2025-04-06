using MediatR;

namespace AppointmentService.Application.Commands.CreateAppointment;

public record CreateAppointmentCommand : IRequest<Guid>
{
    public Guid CustomerId { get; init; }
    public Guid LawyerId { get; init; }
    public DateTime ScheduledAt { get; init; }
    public string Notes { get; init; } = string.Empty;
}
