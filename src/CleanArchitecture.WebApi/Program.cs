using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Presentation;
using CleanArchitecture.Presentation.Controllers;

var builder = WebApplication.CreateBuilder(args);

// --- Composition root: wire the layers (clean-correct dependency direction) ---
builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPresentation();

// Controllers live in the Presentation assembly; register it as an application part.
builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(BaseApiController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

// TODO(Features): replace/extend the wiring below with the PowerCSharp Features framework, e.g.
//   builder.Services.AddPowerFeatures(builder.Configuration, options => { ... });
// Built-in Features planned to migrate here: CORS, correlation ID, security headers,
// response compression, exception-handling middleware, API versioning, JWT auth wiring.

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// TODO(Features): app.UsePowerFeatures(); to apply enabled feature middleware in order.

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

/// <summary>
/// Exposed as a partial class so integration tests can use WebApplicationFactory&lt;Program&gt;.
/// </summary>
public partial class Program
{
}
