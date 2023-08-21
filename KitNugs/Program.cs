using KitNugs.Configuration;
using KitNugs.Logging;
using KitNugs.Repository;
using KitNugs.Services;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Configure logging - we use serilog.
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddScoped<IServiceConfiguration, ServiceConfiguration>();
builder.Services.AddScoped<IHelloService, HelloService>();
builder.Services.AddScoped<ISessionIdAccessor, DefaultSessionIdAccessor>();

builder.Services.AddHttpContextAccessor();

// Add controllers
builder.Services.AddControllers();

// Enable NSwag
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument();

// Setup health checks and Prometheus endpoint
builder.Services.AddHealthChecks()
                .ForwardToPrometheus();

var app = builder.Build();

app.UseMiddleware<LogHeaderMiddleware>();

app.UseHttpMetrics();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseAuthorization();

// Map controllers
app.MapControllers();

// Ensure health endpoint and Prometheus only respond on port 8081
app.UseHealthChecks("/healthz", 8081);
app.UseMetricServer(8081, "/metrics");

using (var scope = app.Services.CreateScope())
{
    // Ensure all env variables is set.
    scope.ServiceProvider.GetRequiredService<IServiceConfiguration>();
}

app.Run();


public partial class Program { }
