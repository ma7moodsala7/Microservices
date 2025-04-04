using Yarp.ReverseProxy;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Load config from yarp.json
builder.Configuration.AddJsonFile("yarp.json", optional: false, reloadOnChange: true);

// Register YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

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

// Add health endpoint
app.MapGet("/health", () => Results.Ok("API Gateway is healthy"));

app.Run();
