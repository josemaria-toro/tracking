using System;
using Zetatech.Accelerate.Domain.Abstractions;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents an application error.
/// </summary>
public sealed class ErrorEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the application identifier.
    /// </summary>
    public Guid AppId { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when error was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public String ErrorTypeName { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the message of the error.
    /// </summary>
    public String Message { get; set; }
    /// <summary>
    /// Gets or sets the operation identifier used to associate related error information.
    /// </summary>
    public Guid OperationId { get; set; }
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
    /// <summary>
    /// Gets or sets the timestamp when error was updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
