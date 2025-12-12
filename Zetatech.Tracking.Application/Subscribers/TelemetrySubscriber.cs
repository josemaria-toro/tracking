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
/// Represents the subscriber to receive and process telemetry messages.
/// </summary>
public sealed class TelemetrySubscriber : RabbitMqSubscriberService<TrackingMessage, RabbitMqSubscriberServiceOptions>, ITelemetrySubscriber
{
    private IDependenciesRepository _dependenciesRepository;
    private IHttpRequestsRepository _httpRequestsRepository;
    private IMetricsRepository _metricsRepository;
    private IPageViewsRepository _pageViewsRepository;
    private ITestsResultsRepository _testsResultsRepository;

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="options">
    /// The configuration options for the messages subscriber.
    /// </param>
    /// <param name="dependenciesRepository">
    /// The repository to manage application dependencies.
    /// </param>
    /// <param name="httpRequestsRepository">
    /// The repository to manage HTTP requests.
    /// </param>
    /// <param name="metricsRepository">
    /// The repository to manage application metrics.
    /// </param>
    /// <param name="pageViewsRepository">
    /// The repository to manage page views.
    /// </param>
    /// <param name="testsResultsRepository">
    /// The repository to manage tests results.
    /// </param>
    /// <param name="trackingService">
    /// Service for tracking application data.
    /// </param>
    /// <param name="jsonSerializerOptions">
    /// Options for objects serializing and deserializing.
    /// </param>
    public TelemetrySubscriber(IOptions<RabbitMqSubscriberServiceOptions> options,
                               IDependenciesRepository dependenciesRepository,
                               IHttpRequestsRepository httpRequestsRepository,
                               IMetricsRepository metricsRepository,
                               IPageViewsRepository pageViewsRepository,
                               ITestsResultsRepository testsResultsRepository,
                               ITrackingService trackingService = null,
                               JsonSerializerOptions jsonSerializerOptions = null) : base(options, trackingService, jsonSerializerOptions)
    {
        _dependenciesRepository = dependenciesRepository;
        _httpRequestsRepository = httpRequestsRepository;
        _metricsRepository = metricsRepository;
        _pageViewsRepository = pageViewsRepository;
        _testsResultsRepository = testsResultsRepository;
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
                    case TrackingMessageTypes.Dependency:
                        await SaveDependencyAsync(message);
                        break;
                    case TrackingMessageTypes.HttpRequest:
                        await SaveHttpRequestAsync(message);
                        break;
                    case TrackingMessageTypes.Metric:
                        await SaveMetricAsync(message);
                        break;
                    case TrackingMessageTypes.PageView:
                        await SavePageViewAsync(message);
                        break;
                    case TrackingMessageTypes.TestResult:
                        await SaveTestResultAsync(message);
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
    private async Task SaveDependencyAsync(TrackingMessage message)
    {
        var dependencyEntity = new DependencyEntity
        {
            CreatedAt = message.Timestamp,
            Id = message.Id,
            InputData = message.Properties.ContainsKey("input") ? message.Properties["input"] : String.Empty,
            Name = message.Properties.ContainsKey("name") ? message.Properties["name"] : throw new ValidationException("The property 'name' is required"),
            OperationId = message.OperationId,
            OutputData = message.Properties.ContainsKey("output") ? message.Properties["output"] : String.Empty,
            TargetName = message.Properties.ContainsKey("target") ? message.Properties["target"] : throw new ValidationException("The property 'target' is required"),
            Type = message.Properties.ContainsKey("type") ? message.Properties["type"] : throw new ValidationException("The property 'type' is required"),
            UpdatedAt = message.Timestamp
        };

        if (!message.Properties.ContainsKey("appId"))
        {
            throw new ValidationException("The property 'appId' is required");
        }

        if (Guid.TryParse(message.Properties["appId"], out var appId))
        {
            dependencyEntity.AppId = appId;
        }
        else
        {
            throw new ValidationException("The property 'appId' has an invalid value");
        }

        if (message.Properties.ContainsKey("duration"))
        {
            if (Double.TryParse(message.Properties["duration"], out var duration))
            {
                dependencyEntity.CreatedAt = dependencyEntity.CreatedAt.AddMilliseconds(-duration);
                dependencyEntity.Duration = duration;
                dependencyEntity.UpdatedAt = dependencyEntity.UpdatedAt.AddMilliseconds(-duration);
            }
            else
            {
                throw new ValidationException("The property 'duration' has an invalid value");
            }
        }

        if (message.Properties.ContainsKey("success"))
        {
            if (Boolean.TryParse(message.Properties["success"], out var success))
            {
                dependencyEntity.Success = success;
            }
            else
            {
                throw new ValidationException("The property 'success' has an invalid value");
            }
        }

        await _dependenciesRepository.InsertAsync(message.OperationId, dependencyEntity);
        await _dependenciesRepository.CommitAsync(message.OperationId);
    }
    private async Task SaveHttpRequestAsync(TrackingMessage message)
    {
        var httpRequestEntity = new HttpRequestEntity
        {
            Body = message.Properties.ContainsKey("body") ? message.Properties["body"] : String.Empty,
            CreatedAt = message.Timestamp,
            Id = message.Id,
            IpAddress = message.Properties.ContainsKey("ipAddress") ? message.Properties["ipAddress"] : String.Empty,
            Name = message.Properties.ContainsKey("name") ? message.Properties["name"] : throw new ValidationException("The property 'ipAddress' is required"),
            OperationId = message.OperationId,
            ResponseBody = message.Properties.ContainsKey("responseBody") ? message.Properties["responseBody"] : String.Empty,
            UpdatedAt = message.Timestamp,
            Url = message.Properties.ContainsKey("uri") ? message.Properties["uri"] : throw new ValidationException("The property 'url' is required")
        };

        if (!message.Properties.ContainsKey("appId"))
        {
            throw new ValidationException("The property 'appId' is required");
        }

        if (Guid.TryParse(message.Properties["appId"], out var appId))
        {
            httpRequestEntity.AppId = appId;
        }
        else
        {
            throw new ValidationException("The property 'appId' has an invalid value");
        }

        if (message.Properties.ContainsKey("duration"))
        {
            if (Double.TryParse(message.Properties["duration"], out var duration))
            {
                httpRequestEntity.CreatedAt = httpRequestEntity.CreatedAt.AddMilliseconds(-duration);
                httpRequestEntity.Duration = duration;
                httpRequestEntity.UpdatedAt = httpRequestEntity.UpdatedAt.AddMilliseconds(-duration);
            }
            else
            {
                throw new ValidationException("The property 'duration' has an invalid value");
            }
        }

        if (message.Properties.ContainsKey("responseCode"))
        {
            if (Int32.TryParse(message.Properties["responseCode"], out var responseCode))
            {
                httpRequestEntity.ResponseCode = responseCode;
            }
            else
            {
                throw new ValidationException("The property 'responseCode' has an invalid value");
            }
        }

        if (message.Properties.ContainsKey("success"))
        {
            if (Boolean.TryParse(message.Properties["success"], out var success))
            {
                httpRequestEntity.Success = success;
            }
            else
            {
                throw new ValidationException("The property 'success' has an invalid value");
            }
        }

        await _httpRequestsRepository.InsertAsync(message.OperationId, httpRequestEntity);
        await _httpRequestsRepository.CommitAsync(message.OperationId);
    }
    private async Task SaveMetricAsync(TrackingMessage message)
    {
        var metricEntity = new MetricEntity
        {
            CreatedAt = message.Timestamp,
            DimensionName = message.Properties.ContainsKey("dimension") ? message.Properties["dimension"] : String.Empty,
            Id = message.Id,
            Name = message.Properties.ContainsKey("name") ? message.Properties["name"] : throw new ValidationException("The property 'name' is required"),
            OperationId = message.OperationId,
            UpdatedAt = message.Timestamp
        };

        if (!message.Properties.ContainsKey("appId"))
        {
            throw new ValidationException("The property 'appId' is required");
        }

        if (Guid.TryParse(message.Properties["appId"], out var appId))
        {
            metricEntity.AppId = appId;
        }
        else
        {
            throw new ValidationException("The property 'appId' has an invalid value");
        }

        if (!message.Properties.ContainsKey("value"))
        {
            throw new ValidationException("The property 'value' is required");
        }

        if (Double.TryParse(message.Properties["value"], out var value))
        {
            metricEntity.Value = value;
        }
        else
        {
            throw new ValidationException("The property 'value' has an invalid value");
        }

        await _metricsRepository.InsertAsync(message.OperationId, metricEntity);
        await _metricsRepository.CommitAsync(message.OperationId);
    }
    private async Task SavePageViewAsync(TrackingMessage message)
    {
        var pageViewEntity = new PageViewEntity
        {
            CreatedAt = message.Timestamp,
            DeviceName = message.Properties.ContainsKey("device") ? message.Properties["device"] : String.Empty,
            Id = message.Id,
            IpAddress = message.Properties.ContainsKey("ipAddress") ? message.Properties["ipAddress"] : String.Empty,
            Name = message.Properties.ContainsKey("name") ? message.Properties["name"] : throw new ValidationException("The property 'value' is required"),
            OperationId = message.OperationId,
            UpdatedAt = message.Timestamp,
            Url = message.Properties.ContainsKey("uri") ? message.Properties["uri"] : String.Empty,
            UserAgent = message.Properties.ContainsKey("userAgent") ? message.Properties["userAgent"] : String.Empty
        };

        if (!message.Properties.ContainsKey("appId"))
        {
            throw new ValidationException("The property 'appId' is required");
        }

        if (Guid.TryParse(message.Properties["appId"], out var appId))
        {
            pageViewEntity.AppId = appId;
        }
        else
        {
            throw new ValidationException("The property 'appId' has an invalid value");
        }

        if (message.Properties.ContainsKey("duration"))
        {
            if (Double.TryParse(message.Properties["duration"], out var duration))
            {
                pageViewEntity.CreatedAt = pageViewEntity.CreatedAt.AddMilliseconds(-duration);
                pageViewEntity.Duration = duration;
                pageViewEntity.UpdatedAt = pageViewEntity.UpdatedAt.AddMilliseconds(-duration);
            }
            else
            {
                throw new ValidationException("The property 'duration' has an invalid value");
            }
        }

        await _pageViewsRepository.InsertAsync(message.OperationId, pageViewEntity);
        await _pageViewsRepository.CommitAsync(message.OperationId);
    }
    private async Task SaveTestResultAsync(TrackingMessage message)
    {
        var testResultEntity = new TestResultEntity
        {
            CreatedAt = message.Timestamp,
            Id = message.Id,
            Message = message.Properties.ContainsKey("message") ? message.Properties["message"] : String.Empty,
            Name = message.Properties.ContainsKey("name") ? message.Properties["name"] : throw new ValidationException("The property 'appId' is required"),
            OperationId = message.OperationId,
            UpdatedAt = message.Timestamp
        };

        if (!message.Properties.ContainsKey("appId"))
        {
            throw new ValidationException("The property 'appId' is required");
        }

        if (Guid.TryParse(message.Properties["appId"], out var appId))
        {
            testResultEntity.AppId = appId;
        }
        else
        {
            throw new ValidationException("The property 'appId' has an invalid value");
        }

        if (message.Properties.ContainsKey("duration"))
        {
            if (Double.TryParse(message.Properties["duration"], out var duration))
            {
                testResultEntity.CreatedAt = testResultEntity.CreatedAt.AddMilliseconds(-duration);
                testResultEntity.Duration = duration;
                testResultEntity.UpdatedAt = testResultEntity.UpdatedAt.AddMilliseconds(-duration);
            }
            else
            {
                throw new ValidationException("The property 'duration' has an invalid value");
            }
        }

        if (message.Properties.ContainsKey("success"))
        {
            if (Boolean.TryParse(message.Properties["success"], out var success))
            {
                testResultEntity.Success = success;
            }
            else
            {
                throw new ValidationException("The property 'success' has an invalid value");
            }
        }

        await _testsResultsRepository.InsertAsync(message.OperationId, testResultEntity);
        await _testsResultsRepository.CommitAsync(message.OperationId);
    }
}
