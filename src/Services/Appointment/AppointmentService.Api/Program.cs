using AppointmentService.Application.Commands.CreateAppointment;
using AppointmentService.Application.Interfaces;
using AppointmentService.Persistence;
using AppointmentService.Persistence.Repositories;
using MediatR;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging;
using Shared.Messaging.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAppointmentCommand).Assembly));

// Add DbContext
builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseInMemoryDatabase("AppointmentDb"));

// Add repository
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

// Add shared MassTransit configuration
builder.Services.AddSharedMassTransit();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Appointments endpoint
app.MapPost("/appointments", async (CreateAppointmentCommand command, IMediator mediator) =>
{
    var appointmentId = await mediator.Send(command);
    return Results.Created($"/appointments/{appointmentId}", appointmentId);
});

app.Run();
