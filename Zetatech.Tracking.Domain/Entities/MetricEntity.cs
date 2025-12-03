using System;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents an application metric.
/// </summary>
public sealed class MetricEntity : TrackingEntity
{
    /// <summary>
    /// Gets or sets the dimension or category associated with the metric.
    /// </summary>
    public String DimensionName { get; set; }
    /// <summary>
    /// Gets or sets the name of the metric.
    /// </summary>
    public String Name { get; set; }
    /// <summary>
    /// Gets or sets the value of the metric.
    /// </summary>
    public Double Value { get; set; }
}