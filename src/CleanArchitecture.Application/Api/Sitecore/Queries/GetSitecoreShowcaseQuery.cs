using Ardalis.Result;
using CleanArchitecture.Application.Api.Sitecore.Responses;
using MediatR;

namespace CleanArchitecture.Application.Api.Sitecore.Queries;

/// <summary>
/// Query that exercises the Axlis facade across five pivot scenarios:
/// TextField, ImageField/MultilistField, ItemReferenceField, Axes traversal,
/// GetDescendants, and the WithResult rich-API.
/// </summary>
public sealed class GetSitecoreShowcaseQuery : IRequest<Result<GetSitecoreShowcaseResponse>>
{
    /// <summary>
    /// The Sitecore path or GUID to use as the root for Axes traversal.
    /// Defaults to the site root.
    /// </summary>
    /// <example>/sitecore/content/home</example>
    // fixme: replace with the real root path for the connected Sitecore instance
    public string RootPath { get; }

    public GetSitecoreShowcaseQuery(string rootPath = "/sitecore/content/home")
    {
        RootPath = rootPath;
    }
}
