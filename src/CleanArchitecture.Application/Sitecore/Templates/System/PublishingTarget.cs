using Axlis.ORM.Attributes;
using Axlis.ORM.Core;
using Axlis.ORM.Core.FieldTypes;

namespace CleanArchitecture.Application.Sitecore.Templates.System;

/// <summary>
/// Represents a publishing target template from Sitecore.
/// Contains information about target databases for publishing operations.
/// </summary>
[SitecoreTemplate("{E130C748-C13B-40D5-B6C6-4B150DC3FAB3}")]
public class PublishingTarget : ExtendedItem
{
    /// <summary>Gets the target database field for publishing.</summary>
    [SitecoreField("Target database")]
    public TextField TargetDatabase => GetField<TextField>("Target database");
}
