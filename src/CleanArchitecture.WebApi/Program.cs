using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Presentation;
using CleanArchitecture.Presentation.Controllers;
using PowerCSharp.BuiltInFeatures;
using PowerCSharp.Feature.Cache;
using PowerCSharp.Feature.Cache.BitFaster;
using PowerCSharp.Features;

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

// --- PowerCSharp Features framework ---
// Discovers feature modules, resolves flags (override > env > appsettings) and self-gates each
// module on its PowerFeatures:<Key>:Enabled flag.
builder.Services
    .AddPowerFeatures(builder.Configuration, options =>
    {
        options.AddBuiltInFeatures();                              // Group 1 bundle (CORS, ...)
        options.ScanAssemblies(typeof(CacheFeatureModule).Assembly); // Group 2 pluggable: Cache
    })
    .AddCacheBitFaster(builder.Configuration);                    // BitFaster provider (flag-gated)

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Applies enabled features' middleware (e.g. CORS) in module Order.
app.UsePowerFeatures();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

/// <summary>
/// Exposed as a partial class so integration tests can use WebApplicationFactory&lt;Program&gt;.
/// </summary>
public partial class Program
{
}
