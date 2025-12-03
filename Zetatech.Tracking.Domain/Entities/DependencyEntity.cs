using System;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents the results of a dependency calls.
/// </summary>
public sealed class DependencyEntity : TrackingEntity
{
    /// <summary>
    /// Gets or sets the duration (in milliseconds) of the dependency call.
    /// </summary>
    public Double Duration { get; set; }
    /// <summary>
    /// Gets or sets the command or data sended to the dependency call.
    /// </summary>
    public String InputData { get; set; }
    /// <summary>
    /// Gets or sets the name of the dependency call.
    /// </summary>
    public String Name { get; set; }
    /// <summary>
    /// Gets or sets the output data returned by the dependency call.
    /// </summary>
    public String OutputData { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the dependency call was successful.
    /// </summary>
    public Boolean Success { get; set; }
    /// <summary>
    /// Gets or sets the target system or endpoint of the dependency call.
    /// </summary>
    public String TargetName { get; set; }
    /// <summary>
    /// Gets or sets the type of the dependency call (e.g., SQL, HTTP, etc.).
    /// </summary>
    public String Type { get; set; }
}