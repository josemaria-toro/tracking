using Zetatech.Accelerate.Messaging;
using Zetatech.Accelerate.Tracking.Services;

namespace Zetatech.Tracking.Application.Contracts;

/// <summary>
/// Provides the interface for implementing custom subscribers for receiving and processing events messages.
/// </summary>
public interface IEventsSubscriber : ISubscriberService<TrackingMessage>
{
}
