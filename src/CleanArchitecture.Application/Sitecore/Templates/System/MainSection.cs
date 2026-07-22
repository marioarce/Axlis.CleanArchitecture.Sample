using Axlis.ORM.Attributes;
using Axlis.ORM.Core;

namespace CleanArchitecture.Application.Sitecore.Templates.System;

/// <summary>
/// Represents a main section template from Sitecore.
/// This is a base system template with no additional fields.
/// </summary>
/// <remarks>
/// This template is used as a foundation for other templates in the system.
/// </remarks>
[SitecoreTemplate("{E3E2D58C-DF95-4230-ADC9-279924CECE84}")]
public class MainSection : ExtendedItem
{
}
