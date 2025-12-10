using System;
using Zetatech.Accelerate.Domain.Abstractions;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents a page view.
/// </summary>
public sealed class PageViewEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the application identifier.
    /// </summary>
    public Guid AppId { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when page view was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the name of the device.
    /// </summary>
    public String DeviceName { get; set; }
    /// <summary>
    /// Gets or sets the duration (in milliseconds) of the page view.
    /// </summary>
    public Double Duration { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the page view.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the ip address of the device.
    /// </summary>
    public String IpAddress { get; set; }
    /// <summary>
    /// Gets or sets the name of the page viewed.
    /// </summary>
    public String Name { get; set; }
    /// <summary>
    /// Gets or sets the operation identifier used to associate related page view information.
    /// </summary>
    public Guid OperationId { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when page view was updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    /// <summary>
    /// Gets or sets the URI of the page viewed.
    /// </summary>
    public String Url { get; set; }
    /// <summary>
    /// Gets or sets the user agent of the device.
    /// </summary>
    public String UserAgent { get; set; }
}