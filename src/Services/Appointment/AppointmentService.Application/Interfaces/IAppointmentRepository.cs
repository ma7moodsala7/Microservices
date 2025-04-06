using AppointmentService.Domain.Entities;

namespace AppointmentService.Application.Interfaces;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment);
    Task SaveChangesAsync();
}
