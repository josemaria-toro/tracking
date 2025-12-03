using System;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents an application event.
/// </summary>
public sealed class EventEntity : TrackingEntity
{
    /// <summary>
    /// Gets or sets the metadata of the event.
    /// </summary>
    public String Metadata { get; set; }
    /// <summary>
    /// Gets or sets the name of the event.
    /// </summary>
    public String Name { get; set; }
}