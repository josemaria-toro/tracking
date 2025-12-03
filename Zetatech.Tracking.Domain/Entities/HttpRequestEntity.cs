using System;

namespace Zetatech.Tracking.Domain.Entities;

/// <summary>
/// Represents an HTTP request.
/// </summary>
public sealed class HttpRequestEntity : TrackingEntity
{
    /// <summary>
    /// Gets or sets the body of the HTTP request.
    /// </summary>
    public String Body { get; set; }
    /// <summary>
    /// Gets or sets the duration (in milliseconds) of the HTTP request.
    /// </summary>
    public Double Duration { get; set; }
    /// <summary>
    /// Gets or sets the IP address from which the HTTP request originated.
    /// </summary>
    public String IpAddress { get; set; }
    /// <summary>
    /// Gets or sets the name or identifier of the HTTP request.
    /// </summary>
    public String Name { get; set; }
    /// <summary>
    /// Gets or sets the response body of the HTTP request.
    /// </summary>
    public String ResponseBody { get; set; }
    /// <summary>
    /// Gets or sets the HTTP response code returned for the HTTP request.
    /// </summary>
    public Int32 ResponseCode { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the HTTP request was successful.
    /// </summary>
    public Boolean Success { get; set; }
    /// <summary>
    /// Gets or sets the URI associated with the HTTP request.
    /// </summary>
    public String Url { get; set; }
}