using AppointmentService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly ILogger<CreateAppointmentCommandHandler> _logger;

    public CreateAppointmentCommandHandler(ILogger<CreateAppointmentCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        // TODO: Validate JWT token (Authorization header)
        
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            LawyerId = request.LawyerId,
            ScheduledAt = request.ScheduledAt,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        // TODO: Add to database context
        // TODO: Add Audit Log entry here
        // TODO: Publish AppointmentCreatedIntegrationEvent

        _logger.LogInformation("Created appointment with ID: {AppointmentId}", appointment.Id);
        
        return appointment.Id;
    }
}
