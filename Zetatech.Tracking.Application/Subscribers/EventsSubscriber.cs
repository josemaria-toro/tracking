using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Zetatech.Accelerate.Exceptions;
using Zetatech.Accelerate.Messaging.Services;
using Zetatech.Accelerate.Tracking;
using Zetatech.Accelerate.Tracking.Services;
using Zetatech.Tracking.Application.Contracts;
using Zetatech.Tracking.Domain.Entities;
using Zetatech.Tracking.Domain.Repositories;

namespace Zetatech.Tracking.Application.Subscribers;

/// <summary>
/// Represents the subscriber to receive and process messages of business events.
/// </summary>
public sealed class EventsSubscriber : RabbitMqSubscriberService<TrackingMessage, RabbitMqSubscriberServiceOptions>, IEventsSubscriber
{
    private IEventsRepository _eventsRepository;

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="options">
    /// The configuration options for the messages subscriber.
    /// </param>
    /// <param name="eventsRepository">
    /// The repository to manage events.
    /// </param>
    /// <param name="trackingService">
    /// Service for tracking application data.
    /// </param>
    /// <param name="jsonSerializerOptions">
    /// Options for objects serializing and deserializing.
    /// </param>
    public EventsSubscriber(IOptions<RabbitMqSubscriberServiceOptions> options,
                            IEventsRepository eventsRepository,
                            ITrackingService trackingService = null,
                            JsonSerializerOptions jsonSerializerOptions = null) : base(options, trackingService, jsonSerializerOptions)
    {
        _eventsRepository = eventsRepository;
    }

    /// <summary>
    /// Notify a new message.
    /// </summary>
    /// <param name="message">
    /// Message information.
    /// </param>
    protected override async void OnMessageReceived(TrackingMessage message)
    {
        if (message != null)
        {
            Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | Message of type '{message.MessageType}' was received");

            try
            {
                switch (message.MessageType)
                {
                    case TrackingMessageTypes.Event:
                        await SaveEventAsync(message);
                        break;
                }
            }
            catch (ValidationException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | Error processing message of type '{message.MessageType}' because is malformed | {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | Error processing message of type '{message.MessageType}' | {ex.Message}");
                Console.ResetColor();
            }
        }
    }
    private async Task SaveEventAsync(TrackingMessage message)
    {
        var eventEntity = new EventEntity
        {
            CreatedAt = message.Timestamp,
            Id = message.Id,
            Metadata = message.Properties.ContainsKey("metadata") ? message.Properties["metadata"] : String.Empty,
            Name = message.Properties.ContainsKey("name") ? message.Properties["name"] : throw new ValidationException("The property 'name' is required"),
            OperationId = message.OperationId,
            UpdatedAt = message.Timestamp,
        };

        if (!message.Properties.ContainsKey("appId"))
        {
            throw new ValidationException("The property 'appId' is required");
        }

        if (Guid.TryParse(message.Properties["appId"], out var appId))
        {
            eventEntity.AppId = appId;
        }
        else
        {
            throw new ValidationException("The property 'appId' has an invalid value");
        }

        await _eventsRepository.InsertAsync(message.OperationId, eventEntity);
        await _eventsRepository.CommitAsync(message.OperationId);
    }
}
