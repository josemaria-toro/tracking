using System;
using Zetatech.Accelerate.Domain.Abstractions;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents an application event.
/// </summary>
public sealed class EventEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the application identifier.
    /// </summary>
    public Guid AppId { get; set; }
    /// <summary>
    /// Gets or sets the metadata of the event.
    /// </summary>
    public String Metadata { get; set; }
    /// <summary>
    /// Gets or sets the name of the event.
    /// </summary>
    public String Name { get; set; }
    /// <summary>
    /// Gets or sets the operation identifier used to associate related event information.
    /// </summary>
    public Guid OperationId { get; set; }
}