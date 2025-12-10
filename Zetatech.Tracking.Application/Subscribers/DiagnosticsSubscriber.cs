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
/// Represents the subscriber to receive and process diagnostics messages.
/// </summary>
public sealed class DiagnosticsSubscriber : RabbitMqSubscriberService<TrackingMessage, RabbitMqSubscriberServiceOptions>, IDiagnosticsSubscriber
{
    private IErrorsRepository _errorsRepository;
    private ITracesRepository _tracesRepository;

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="options">
    /// The configuration options for the messages subscriber.
    /// </param>
    /// <param name="errorsRepository">
    /// The repository to manage application errors.
    /// </param>
    /// <param name="tracesRepository">
    /// The repository to manage application traces.
    /// </param>
    /// <param name="trackingService">
    /// Service for tracking application data.
    /// </param>
    /// <param name="jsonSerializerOptions">
    /// Options for objects serializing and deserializing.
    /// </param>
    public DiagnosticsSubscriber(IOptions<RabbitMqSubscriberServiceOptions> options,
                                 IErrorsRepository errorsRepository,
                                 ITracesRepository tracesRepository,
                                 ITrackingService trackingService = null,
                                 JsonSerializerOptions jsonSerializerOptions = null) : base(options, trackingService, jsonSerializerOptions)
    {
        _errorsRepository = errorsRepository;
        _tracesRepository = tracesRepository;
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

            switch (message.MessageType)
            {
                case TrackingMessageTypes.Error:
                    await SaveErrorAsync(message);
                    break;
                case TrackingMessageTypes.Trace:
                    await SaveTraceAsync(message);
                    break;
            }
        }
    }
    private async Task SaveErrorAsync(TrackingMessage message)
    {
        var errorEntity = new ErrorEntity
        {
            CreatedAt = message.Timestamp,
            ErrorTypeName = message.Properties.ContainsKey("errorTypeName") ? message.Properties["errorTypeName"] : throw new ValidationException("The property 'errorTypeName' is required"),
            Id = message.Id,
            Message = message.Properties.ContainsKey("message") ? message.Properties["message"] : throw new ValidationException("The property 'message' is required"),
            OperationId = message.OperationId,
            SourceTypeName = message.Properties.ContainsKey("sourceTypeName") ? message.Properties["sourceTypeName"] : throw new ValidationException("The property 'sourceTypeName' is required"),
            StackTrace = message.Properties.ContainsKey("stackTrace") ? message.Properties["stackTrace"] : String.Empty,
            UpdatedAt = message.Timestamp
        };

        if (!message.Properties.ContainsKey("appId"))
        {
            throw new ValidationException("The property 'appId' is required");
        }

        if (Guid.TryParse(message.Properties["appId"], out var appId))
        {
            errorEntity.AppId = appId;
        }
        else
        {
            throw new ValidationException("The property 'appId' has an invalid value");
        }

        if (!message.Properties.ContainsKey("severity"))
        {
            throw new ValidationException("The property 'severity' is required");
        }

        if (Enum.TryParse<Severities>(message.Properties["severity"], out var severity))
        {
            errorEntity.Severity = (Int32)severity;
        }
        else
        {
            throw new ValidationException("The property 'severity' has an invalid value");
        }

        await _errorsRepository.InsertAsync(errorEntity);
        await _errorsRepository.CommitAsync();
    }
    private async Task SaveTraceAsync(TrackingMessage message)
    {
        var traceEntity = new TraceEntity
        {
            CreatedAt = message.Timestamp,
            Id = message.Id,
            Message = message.Properties.ContainsKey("message") ? message.Properties["message"] : throw new ValidationException("The property 'message' is required"),
            OperationId = message.OperationId,
            SourceTypeName = message.Properties.ContainsKey("sourceTypeName") ? message.Properties["sourceTypeName"] : throw new ValidationException("The property 'sourceTypeName' is required"),
            UpdatedAt = message.Timestamp
        };

        if (!message.Properties.ContainsKey("appId"))
        {
            throw new ValidationException("The property 'appId' is required");
        }

        if (Guid.TryParse(message.Properties["appId"], out var appId))
        {
            traceEntity.AppId = appId;
        }
        else
        {
            throw new ValidationException("The property 'appId' has an invalid value");
        }

        if (!message.Properties.ContainsKey("severity"))
        {
            throw new ValidationException("The property 'severity' is required");
        }

        if (Enum.TryParse<Severities>(message.Properties["severity"], out var severity))
        {
            traceEntity.Severity = (Int32)severity;
        }
        else
        {
            throw new ValidationException("The property 'severity' has an invalid value");
        }

        await _tracesRepository.InsertAsync(traceEntity);
        await _tracesRepository.CommitAsync();
    }
}
