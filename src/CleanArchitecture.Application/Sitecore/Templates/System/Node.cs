using Axlis.ORM.Attributes;
using Axlis.ORM.Core;

namespace CleanArchitecture.Application.Sitecore.Templates.System;

/// <summary>
/// Represents a node template from Sitecore.
/// This is a base system template with no additional fields.
/// </summary>
/// <remarks>
/// This template is used as a foundation for hierarchical structures in Sitecore.
/// </remarks>
[SitecoreTemplate("{239F9CF4-E5A0-44E0-B342-0F32CD4C6D8B}")]
public class Node : ExtendedItem
{
}
