using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using PowerCSharp.Features.Abstractions;

namespace CleanArchitecture.Presentation.Api.Attributes;

/// <summary>
/// Gates a controller or action behind a PowerCSharp feature flag. When the feature is disabled,
/// the endpoint responds with 404 so its existence is hidden (e.g. in Production).
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class FeatureGateAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _featureKey;

    public FeatureGateAttribute(string featureKey)
    {
        _featureKey = featureKey;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var flags = context.HttpContext.RequestServices.GetService<IFeatureFlagProvider>();

        if (flags is null || !flags.IsEnabled(_featureKey))
        {
            context.Result = new NotFoundResult();
            return;
        }

        await next().ConfigureAwait(false);
    }
}
