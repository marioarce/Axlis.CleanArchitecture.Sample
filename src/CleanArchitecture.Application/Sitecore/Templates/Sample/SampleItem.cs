using Axlis.Attributes;
using Axlis.Core;
using Axlis.Core.FieldTypes;

namespace CleanArchitecture.Application.Sitecore.Templates.Sample;

/// <summary>Sample item template from Sitecore.</summary>
[SitecoreTemplate("{76036F5E-C477-44E2-8178-773413C533F7}")]
public class SampleItem : ExtendedItem
{
    /// <summary>Title field.</summary>
    [SitecoreField("Title")]
    public TextField Title => GetField<TextField>("Title");

    /// <summary>Text field.</summary>
    [SitecoreField("Text")]
    public TextField Text => GetField<TextField>("Text");
}
