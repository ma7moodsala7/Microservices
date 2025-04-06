using MediatR;

namespace AppointmentService.Application.Commands;

public class CreateAppointmentCommand : IRequest<Guid>
{
    public Guid CustomerId { get; set; }
    public Guid LawyerId { get; set; }
    public DateTime ScheduledAt { get; set; }
}
