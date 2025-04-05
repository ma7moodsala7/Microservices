using IdentityService.Domain.Entities;
using IdentityService.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using IdentityService.Application.Features.Auth.Commands;
using Shared.Logging;
using Shared.Messaging;
using Shared.Messaging.Events;
using IdentityService.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

// Add MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserPingedConsumer>();

    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

// Register message publisher
builder.Services.AddScoped<IMessagePublisher, MassTransitPublisher>();

// Configure Serilog
builder.Host.UseSharedSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

// Add authentication/authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapDefaultControllerRoute();

// Seed test user
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    var existingUser = await userManager.FindByEmailAsync("test@shwra.com");
    if (existingUser == null)
    {
        var user = new User
        {
            UserName = "test@shwra.com",
            Email = "test@shwra.com",
            PhoneNumber = "+966501234567",
            EmailConfirmed = true, // For testing purposes
            FirstName = "Test",
            LastName = "User"
        };

        var result = await userManager.CreateAsync(user, "Shwra123$");
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create test user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}

app.MapPost("/identity/test/publish", async (IMessagePublisher publisher) =>
{
    var evt = new UserPingedIntegrationEvent
    {
        UserId = "test-user",
        Message = "Hello from IdentityService!"
    };

    await publisher.PublishAsync(evt);

    return Results.Ok("Published UserPingedIntegrationEvent.");
});

app.Run();