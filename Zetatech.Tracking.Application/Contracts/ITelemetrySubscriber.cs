using Zetatech.Accelerate.Messaging;
using Zetatech.Accelerate.Tracking.Services;

namespace Zetatech.Tracking.Application.Contracts;

/// <summary>
/// Provides the interface for implementing custom subscribers for receiving and processing telemetry messages.
/// </summary>
public interface ITelemetrySubscriber : ISubscriberService<TrackingMessage>
{
}
