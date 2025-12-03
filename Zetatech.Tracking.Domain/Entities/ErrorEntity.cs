using System;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents an application error.
/// </summary>
public sealed class ErrorEntity : TrackingEntity
{
    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public String ErrorTypeName { get; set; }
    /// <summary>
    /// Gets or sets the message of the error.
    /// </summary>
    public String Message { get; set; }
    /// <summary>
    /// Gets or sets the severity of the error.
    /// </summary>
    public Int32 Severity { get; set; }
    /// <summary>
    /// Gets or sets the source of the error.
    /// </summary>
    public String SourceTypeName { get; set; }
    /// <summary>
    /// Gets or sets the stack trace.
    /// </summary>
    public String StackTrace { get; set; }
}
