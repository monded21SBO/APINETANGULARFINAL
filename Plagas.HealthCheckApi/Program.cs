using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Plagas.HealthCheckApi.HealthChecks;
using Plagas.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<PlagasDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlagasDb"));
});

builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "api" })
    //
    .AddTypeActivatedCheck<PingHealthCheck>("Firebase", HealthStatus.Degraded, tags: new[] { "api" },
        args: "firebase.com")
    .AddTypeActivatedCheck<PingHealthCheck>("Azure", HealthStatus.Unhealthy, tags: new[] { "api" }, args: "azure.com")
    .AddTypeActivatedCheck<PingHealthCheck>("desarrolloshn", HealthStatus.Unhealthy, tags: new[] { "api" }, args: "desarrolloshn.com")
    .AddSqlServer(builder.Configuration.GetConnectionString("PlagasDb")!, name: "BD 01", failureStatus: HealthStatus.Unhealthy, tags: new[] { "database" })
    .AddSqlServer(builder.Configuration.GetConnectionString("Bd2")!, name: "BD 02", failureStatus: HealthStatus.Unhealthy, tags: new[] { "database" });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    Predicate = x => x.Tags.Contains("api")
});

app.MapHealthChecks("/health/databases", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    Predicate = x => x.Tags.Contains("database")
});

app.MapHealthChecksUI();

app.Run();
