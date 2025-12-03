using System;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents a page view.
/// </summary>
public sealed class PageViewEntity : TrackingEntity
{
    /// <summary>
    /// Gets or sets the name of the device.
    /// </summary>
    public String DeviceName { get; set; }
    /// <summary>
    /// Gets or sets the duration (in milliseconds) of the page view.
    /// </summary>
    public Double Duration { get; set; }
    /// <summary>
    /// Gets or sets the ip address of the device.
    /// </summary>
    public String IpAddress { get; set; }
    /// <summary>
    /// Gets or sets the name of the page viewed.
    /// </summary>
    public String Name { get; set; }
    /// <summary>
    /// Gets or sets the URI of the page viewed.
    /// </summary>
    public String Url { get; set; }
    /// <summary>
    /// Gets or sets the user agent of the device.
    /// </summary>
    public String UserAgent { get; set; }
}