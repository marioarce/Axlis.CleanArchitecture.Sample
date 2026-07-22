using Axlis.ORM.Extensions;
using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Presentation;
using CleanArchitecture.Presentation.Controllers;
using CleanArchitecture.WebApi.Features;
using PowerCSharp.BuiltInFeatures;
using PowerCSharp.Feature.Cache;
using PowerCSharp.Feature.Cache.BitFaster;
using PowerCSharp.Feature.Cache.Disk;
using PowerCSharp.Features;

var builder = WebApplication.CreateBuilder(args);

// --- PowerCSharp Features framework ---
// Discovers feature modules, resolves flags (override > env > appsettings) and self-gates each
// module on its PowerFeatures:<Key>:Enabled flag.
// NOTE: Must be called before AddApplication() because Application layer registers
// SampleCacheFactoryContext which depends on ICacheService registered by this framework.
builder.Services
    .AddPowerFeatures(builder.Configuration, options =>
    {
        options.AddBuiltInFeatures();                                   // Group 1 bundle (CORS, ...)
        options.ScanAssemblies(typeof(CacheFeatureModule).Assembly);    // Group 2 pluggable: Cache
        options.AddModule(new SamplesFeatureModule());                  // Host-local: /v1/samples area
    })
    .AddCacheBitFaster(builder.Configuration)                           // BitFaster provider (flag-gated)
    .AddCacheDisk(builder.Configuration)                                // Disk cache provider (flag-gated)
;

// --- Axlis: Sitecore Headless GraphQL ORM ---
// Note: Endpoint and ApiKey must be set in user-secrets for development
builder.Services
    .AddAxlisORM(o =>
    {
        builder.Configuration.GetSection("Axlis").Bind(o);
    })
    .AddAxlisORMGraphQL(o =>
    {
        o.Endpoint = builder.Configuration["AxlisGraphQL:Endpoint"] ?? string.Empty;
        o.ApiKey   = builder.Configuration["AxlisGraphQL:ApiKey"];
        o.BatchSize = int.TryParse(builder.Configuration["AxlisGraphQL:BatchSize"], out var b) ? b : 10;
        o.TimeoutSeconds = int.TryParse(builder.Configuration["AxlisGraphQL:TimeoutSeconds"], out var t) ? t : 30;
    });

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

var app = builder.Build();

// Wire the ambient lazy-loader so ExtendedItem.Axes traversal works at runtime.
app.Services.UseAxlis();

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
