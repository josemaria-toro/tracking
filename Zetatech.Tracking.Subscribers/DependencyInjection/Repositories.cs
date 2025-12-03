using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Zetatech.Accelerate.Persistency.Repositories;
using Zetatech.Tracking.Domain.Repositories;
using Zetatech.Tracking.Infrastructure.Persistency;

namespace Zetatech.Tracking.DependencyInjection;

/// <summary>
/// Extension methods to configure the dependency injection.
/// </summary>
public static partial class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds and configure the repositories into the service collection descriptors.
    /// </summary>
    /// <param name="serviceCollection">
    /// Collection of service descriptors.
    /// </param>
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IDependenciesRepository, DependenciesRepository>();
        serviceCollection.AddTransient<IErrorsRepository, ErrorsRepository>();
        serviceCollection.AddTransient<IEventsRepository, EventsRepository>();
        serviceCollection.AddTransient<IHttpRequestsRepository, HttpRequestsRepository>();
        serviceCollection.AddTransient<IMetricsRepository, MetricsRepository>();
        serviceCollection.AddTransient<IPageViewsRepository, PageViewsRepository>();
        serviceCollection.AddTransient<ITestsResultsRepository, TestsResultsRepository>();
        serviceCollection.AddTransient<ITracesRepository, TracesRepository>();

        serviceCollection.Configure<PostgreSqlRepositoryOptions>(options =>
        {
            var configurationManager = serviceCollection.BuildServiceProvider()
                                                        .GetRequiredService<IConfiguration>();

            options.ConnectionString = configurationManager.GetConnectionString("tracking");
            options.DetailedErrors = configurationManager.GetValue<Boolean>("tracking:detailedErrors", false);
            options.LazyLoading = configurationManager.GetValue<Boolean>("tracking:lazyLoading", true);
            options.Schema = configurationManager.GetValue<String>("tracking:schema", "tracking");
            options.SensitiveDataLogging = configurationManager.GetValue<Boolean>("tracking:sensitiveDataLogging", true);
            options.Timeout = configurationManager.GetValue<Int32>("tracking:timeout", 30);
            options.TrackChanges = configurationManager.GetValue<Boolean>("tracking:trackChanges", true);
        });

        return serviceCollection;
    }
}