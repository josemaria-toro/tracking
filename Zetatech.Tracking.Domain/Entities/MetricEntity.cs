using System;
using Zetatech.Accelerate.Domain.Abstractions;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents an application metric.
/// </summary>
public sealed class MetricEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the application identifier.
    /// </summary>
    public Guid AppId { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when metric was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the dimension or category associated with the metric.
    /// </summary>
    public String DimensionName { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the metric.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the name of the metric.
    /// </summary>
    public String Name { get; set; }
    /// <summary>
    /// Gets or sets the operation identifier used to associate related metric information.
    /// </summary>
    public Guid OperationId { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when metric was updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    /// <summary>
    /// Gets or sets the value of the metric.
    /// </summary>
    public Double Value { get; set; }
}