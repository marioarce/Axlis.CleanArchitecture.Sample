using Axlis.Attributes;
using Axlis.Core;
using Axlis.Core.FieldTypes;

namespace CleanArchitecture.Application.Sitecore.Templates.System;

/// <summary>Publishing target template from Sitecore.</summary>
[SitecoreTemplate("{E130C748-C13B-40D5-B6C6-4B150DC3FAB3}")]
public class PublishingTarget : ExtendedItem
{
    /// <summary>Target database field.</summary>
    [SitecoreField("Target database")]
    public TextField TargetDatabase => GetField<TextField>("Target database");
}
