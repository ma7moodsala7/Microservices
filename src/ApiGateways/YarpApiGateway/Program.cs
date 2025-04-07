using Yarp.ReverseProxy;
using System.Diagnostics;
using Shared.Logging;
using Shared.Settings;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Load shared configuration
builder.Configuration.AddSharedSettings();

// Load config from yarp.json
builder.Configuration.AddJsonFile("yarp.json", optional: false, reloadOnChange: true);

// Register YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add health checks
builder.Services.AddHealthChecks();

// Add OpenTelemetry
builder.Services.AddOpenTelemetrySupport(builder.Configuration, "api-gateway");

var app = builder.Build();

// Add request logging middleware
app.Use(async (context, next) =>
{
    var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
    var method = context.Request.Method;
    var path = context.Request.Path;

    Console.WriteLine($"[GATEWAY] {method} {path} | TraceId: {traceId}");

    await next();
});

app.MapReverseProxy();

// Add health endpoints
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false, // just confirms the app is running
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Gateway has no dependencies to check for readiness

app.Run();
