using System;
using Zetatech.Accelerate.Domain.Abstractions;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents the result of a test.
/// </summary>
public sealed class TestResultEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the application identifier.
    /// </summary>
    public Guid AppId { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when test result was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the duration (in milliseconds) of the test.
    /// </summary>
    public Double Duration { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the test result.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the message associated with the test result.
    /// </summary>
    public String Message { get; set; }
    /// <summary>
    /// Gets or sets the name of the test.
    /// </summary>
    public String Name { get; set; }
    /// <summary>
    /// Gets or sets the operation identifier used to associate related test result information.
    /// </summary>
    public Guid OperationId { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the test was successful.
    /// </summary>
    public Boolean Success { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when test result was updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}