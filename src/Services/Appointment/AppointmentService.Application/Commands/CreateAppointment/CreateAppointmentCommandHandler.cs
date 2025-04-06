using AppointmentService.Domain.Entities;
using AppointmentService.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging;
using Shared.Messaging.Events;

namespace AppointmentService.Application.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<CreateAppointmentCommandHandler> _logger;

    public CreateAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        IMessagePublisher messagePublisher,
        ILogger<CreateAppointmentCommandHandler> logger)
    {
        _appointmentRepository = appointmentRepository;
        _messagePublisher = messagePublisher;
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

        // Save to database
        await _appointmentRepository.AddAsync(appointment);
        await _appointmentRepository.SaveChangesAsync();

        _logger.LogInformation("Created appointment with ID: {AppointmentId}", appointment.Id);

        // Publish the integration event
        var integrationEvent = new AppointmentCreatedIntegrationEvent
        {
            AppointmentId = appointment.Id,
            CustomerId = appointment.CustomerId,
            LawyerId = appointment.LawyerId,
            ScheduledAt = appointment.ScheduledAt
        };

        _logger.LogInformation(
            "Publishing AppointmentCreatedIntegrationEvent for appointment {AppointmentId}",
            appointment.Id);

        await _messagePublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation(
            "Successfully published AppointmentCreatedIntegrationEvent for appointment {AppointmentId}",
            appointment.Id);

        return appointment.Id;
    }
}
