using System;
using Zetatech.Accelerate.Domain.Abstractions;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents an application trace.
/// </summary>
public sealed class TraceEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the application identifier.
    /// </summary>
    public Guid AppId { get; set; }
    /// <summary>
    /// Gets or sets the message of the trace.
    /// </summary>
    public String Message { get; set; }
    /// <summary>
    /// Gets or sets the operation identifier used to associate related trace information.
    /// </summary>
    public Guid OperationId { get; set; }
    /// <summary>
    /// Gets or sets the severity of the trace.
    /// </summary>
    public Int32 Severity { get; set; }
    /// <summary>
    /// Gets or sets the source of the trace.
    /// </summary>
    public String SourceTypeName { get; set; }
}
