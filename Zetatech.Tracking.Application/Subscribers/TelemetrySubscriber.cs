using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
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
    protected override void OnMessageReceived(TrackingMessage message)
    {
        if (message != null)
        {
            switch (message.MessageType)
            {
                case TrackingMessageTypes.Dependency:
                    SaveDependency(message);
                    break;
                case TrackingMessageTypes.HttpRequest:
                    SaveHttpRequest(message);
                    break;
                case TrackingMessageTypes.Metric:
                    SaveMetric(message);
                    break;
                case TrackingMessageTypes.PageView:
                    SavePageView(message);
                    break;
                case TrackingMessageTypes.TestResult:
                    SaveTestResult(message);
                    break;
            }
        }
    }
    private void SaveDependency(TrackingMessage message)
    {
        var dependencyEntity = new DependencyEntity
        {
            Id = message.Id,
            InputData = message.Properties.ContainsKey("input") ? message.Properties["input"] : String.Empty,
            Name = message.Properties.ContainsKey("name") ? message.Properties["name"] : throw new ValidationException("The property 'name' is required"),
            OperationId = message.OperationId,
            OutputData = message.Properties.ContainsKey("output") ? message.Properties["output"] : String.Empty,
            TargetName = message.Properties.ContainsKey("target") ? message.Properties["target"] : throw new ValidationException("The property 'target' is required"),
            Timestamp = message.Timestamp,
            Type = message.Properties.ContainsKey("type") ? message.Properties["type"] : throw new ValidationException("The property 'type' is required")
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
                dependencyEntity.Duration = duration;
                dependencyEntity.Timestamp = dependencyEntity.Timestamp.AddMilliseconds(-duration);
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

        _dependenciesRepository.Insert(dependencyEntity);
        _dependenciesRepository.Commit();
    }
    private void SaveHttpRequest(TrackingMessage message)
    {
        var httpRequestEntity = new HttpRequestEntity
        {
            Body = message.Properties.ContainsKey("body") ? message.Properties["body"] : String.Empty,
            Id = message.Id,
            IpAddress = message.Properties.ContainsKey("ipAddress") ? message.Properties["ipAddress"] : String.Empty,
            Name = message.Properties.ContainsKey("name") ? message.Properties["name"] : throw new ValidationException("The property 'ipAddress' is required"),
            OperationId = message.OperationId,
            ResponseBody = message.Properties.ContainsKey("responseBody") ? message.Properties["responseBody"] : String.Empty,
            Url = message.Properties.ContainsKey("url") ? message.Properties["url"] : throw new ValidationException("The property 'url' is required"),
            Timestamp = message.Timestamp
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
                httpRequestEntity.Duration = duration;
                httpRequestEntity.Timestamp = httpRequestEntity.Timestamp.AddMilliseconds(-duration);
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

        _httpRequestsRepository.Insert(httpRequestEntity);
        _httpRequestsRepository.Commit();
    }
    private void SaveMetric(TrackingMessage message)
    {
        var metricEntity = new MetricEntity
        {
            DimensionName = message.Properties.ContainsKey("dimension") ? message.Properties["dimension"] : String.Empty,
            Id = message.Id,
            Name = message.Properties.ContainsKey("name") ? message.Properties["name"] : throw new ValidationException("The property 'name' is required"),
            OperationId = message.OperationId,
            Timestamp = message.Timestamp,
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

        _metricsRepository.Insert(metricEntity);
        _metricsRepository.Commit();
    }
    private void SavePageView(TrackingMessage message)
    {
        var pageViewEntity = new PageViewEntity
        {
            DeviceName = message.Properties.ContainsKey("device") ? message.Properties["device"] : String.Empty,
            Id = message.Id,
            IpAddress = message.Properties.ContainsKey("ipAddress") ? message.Properties["ipAddress"] : String.Empty,
            Name = message.Properties.ContainsKey("name") ? message.Properties["name"] : throw new ValidationException("The property 'value' is required"),
            OperationId = message.OperationId,
            Timestamp = message.Timestamp,
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
                pageViewEntity.Duration = duration;
                pageViewEntity.Timestamp = pageViewEntity.Timestamp.AddMilliseconds(-duration);
            }
            else
            {
                throw new ValidationException("The property 'duration' has an invalid value");
            }
        }

        _pageViewsRepository.Insert(pageViewEntity);
        _pageViewsRepository.Commit();
    }
    private void SaveTestResult(TrackingMessage message)
    {
        var testResultEntity = new TestResultEntity
        {
            Id = message.Id,
            Message = message.Properties.ContainsKey("message") ? message.Properties["message"] : String.Empty,
            Name = message.Properties.ContainsKey("name") ? message.Properties["name"] : throw new ValidationException("The property 'appId' is required"),
            OperationId = message.OperationId,
            Timestamp = message.Timestamp
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
                testResultEntity.Duration = duration;
                testResultEntity.Timestamp = testResultEntity.Timestamp.AddMilliseconds(-duration);
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

        _testsResultsRepository.Insert(testResultEntity);
        _testsResultsRepository.Commit();
    }
}
