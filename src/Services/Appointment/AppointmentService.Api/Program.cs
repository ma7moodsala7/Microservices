using AppointmentService.Application.Commands.CreateAppointment;
using AppointmentService.Application.Interfaces;
using AppointmentService.Persistence;
using Shared.Settings;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using AppointmentService.Persistence.Repositories;
using MediatR;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging;
using Shared.Messaging.Events;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

// Load shared configuration
builder.Configuration.AddSharedSettings();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add OpenTelemetry
builder.Services.AddOpenTelemetrySupport(builder.Configuration, "AppointmentService");

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAppointmentCommand).Assembly));

// Add DbContext
builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseInMemoryDatabase("AppointmentDb"));

// Add repository
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

// Add shared MassTransit configuration
builder.Services.AddSharedMassTransit();

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppointmentDbContext>("db", tags: new[] { "ready" });

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

// Add health endpoints
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false, // just confirms the app is running
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
