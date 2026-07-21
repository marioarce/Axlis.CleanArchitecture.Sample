using Axlis.ORM.Attributes;
using Axlis.ORM.Core;
using Axlis.ORM.Core.FieldTypes;

namespace CleanArchitecture.Application.Sitecore.Templates.Sample;

/// <summary>
/// Represents a sample item template from Sitecore.
/// This template demonstrates the usage of Axlis for defining Sitecore templates.
/// </summary>
[SitecoreTemplate("{76036F5E-C477-44E2-8178-773413C533F7}")]
public class SampleItem : ExtendedItem
{
    /// <summary>Gets the title field of the sample item.</summary>
    [SitecoreField("Title")]
    public TextField Title => GetField<TextField>("Title");

    /// <summary>Gets the text field of the sample item.</summary>
    [SitecoreField("Text")]
    public TextField Text => GetField<TextField>("Text");
}
