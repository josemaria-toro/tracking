using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Zetatech.Accelerate.Messaging.Services;
using Zetatech.Tracking.Application.Contracts;
using Zetatech.Tracking.Application.Subscribers;
using Zetatech.Tracking.Domain.Repositories;

namespace Zetatech.Tracking.DependencyInjection;

/// <summary>
/// Extension methods to configure the dependency injection.
/// </summary>
public static partial class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds and configure the message subscribers into the service collection descriptors.
    /// </summary>
    /// <param name="serviceCollection">
    /// Collection of service descriptors.
    /// </param>
    public static IServiceCollection AddSubscribers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IDiagnosticsSubscriber>(serviceProvider =>
        {
            var configurationManager = serviceProvider.GetRequiredService<IConfiguration>();
            var subscriberServiceOptions = Options.Create(new RabbitMqSubscriberServiceOptions
            {
                ConnectionString = configurationManager.GetConnectionString("messageBroker"),
                Exclusive = configurationManager.GetValue<Boolean>("diagnostics:exclusive", true),
                QueueName = configurationManager.GetValue<String>("diagnostics:queueName", "diagnostics")
            });

            return new DiagnosticsSubscriber(
                subscriberServiceOptions,
                serviceProvider.GetRequiredService<IErrorsRepository>(),
                serviceProvider.GetRequiredService<ITracesRepository>()
            );
        });

        serviceCollection.AddTransient<IEventsSubscriber>(serviceProvider =>
        {
            var configurationManager = serviceProvider.GetRequiredService<IConfiguration>();
            var subscriberServiceOptions = Options.Create(new RabbitMqSubscriberServiceOptions
            {
                ConnectionString = configurationManager.GetConnectionString("messageBroker"),
                Exclusive = configurationManager.GetValue<Boolean>("events:exclusive", true),
                QueueName = configurationManager.GetValue<String>("events:queueName", "events")
            });

            return new EventsSubscriber(
                subscriberServiceOptions,
                serviceProvider.GetRequiredService<IEventsRepository>()
            );
        });

        serviceCollection.AddTransient<ITelemetrySubscriber>(serviceProvider =>
        {
            var configurationManager = serviceProvider.GetRequiredService<IConfiguration>();
            var subscriberServiceOptions = Options.Create(new RabbitMqSubscriberServiceOptions
            {
                ConnectionString = configurationManager.GetConnectionString("messageBroker"),
                Exclusive = configurationManager.GetValue<Boolean>("telemetry:exclusive", true),
                QueueName = configurationManager.GetValue<String>("telemetry:queueName", "telemetry")
            });

            return new TelemetrySubscriber(
                subscriberServiceOptions,
                serviceProvider.GetRequiredService<IDependenciesRepository>(),
                serviceProvider.GetRequiredService<IHttpRequestsRepository>(),
                serviceProvider.GetRequiredService<IMetricsRepository>(),
                serviceProvider.GetRequiredService<IPageViewsRepository>(),
                serviceProvider.GetRequiredService<ITestsResultsRepository>()
            );
        });

        return serviceCollection;
    }
}