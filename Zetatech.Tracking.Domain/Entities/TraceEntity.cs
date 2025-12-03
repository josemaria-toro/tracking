using System;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents an application trace.
/// </summary>
public sealed class TraceEntity : TrackingEntity
{
    /// <summary>
    /// Gets or sets the message of the trace.
    /// </summary>
    public String Message { get; set; }
    /// <summary>
    /// Gets or sets the severity of the trace.
    /// </summary>
    public Int32 Severity { get; set; }
    /// <summary>
    /// Gets or sets the source of the trace.
    /// </summary>
    public String SourceTypeName { get; set; }
}
