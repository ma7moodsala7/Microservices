using MediatR;
using Shared.Messaging;
using Shared.Messaging.Events;
using AppointmentService.Domain.Entities;
using AppointmentService.Domain.Enums;
using AppointmentService.Application.Commands;
using AppointmentService.Application.Interfaces;

namespace AppointmentService.Application.Handlers;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMessagePublisher _messagePublisher;

    public CreateAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        IMessagePublisher messagePublisher)
    {
        _appointmentRepository = appointmentRepository;
        _messagePublisher = messagePublisher;
    }

    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            LawyerId = request.LawyerId,
            ScheduledAt = request.ScheduledAt,
            Status = AppointmentStatus.Scheduled,
            CreatedAt = DateTime.UtcNow
        };

        await _appointmentRepository.AddAsync(appointment);
        await _appointmentRepository.SaveChangesAsync();

        // Publish the integration event
        await _messagePublisher.PublishAsync(new AppointmentCreatedIntegrationEvent
        {
            AppointmentId = appointment.Id,
            CustomerId = appointment.CustomerId,
            LawyerId = appointment.LawyerId,
            ScheduledAt = appointment.ScheduledAt
        });

        return appointment.Id;
    }
}
