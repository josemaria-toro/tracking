using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Application.DependencyInjection;
using Zetatech.Accelerate.Messaging;
using Zetatech.Tracking.Application.Contracts;
using Zetatech.Tracking.DependencyInjection;

namespace Zetatech.Tracking;

/// <summary>
/// The main class of the application.
/// </summary>
public static class Program
{
    /// <summary>
    /// The entry point of the application.
    /// </summary>
    /// <param name="arguments">
    /// The command-line arguments.
    /// </param>
    public static async Task Main(String[] arguments)
    {
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.AddConfigurationService()
                                               .AddRepositories()
                                               .AddSubscribers()
                                               .BuildServiceProvider();

        await serviceProvider.GetRequiredService<IDiagnosticsSubscriber>()
                             .SubscribeAsync();
        await serviceProvider.GetRequiredService<IEventsSubscriber>()
                             .SubscribeAsync();
        await serviceProvider.GetRequiredService<ITelemetrySubscriber>()
                             .SubscribeAsync();

        await Task.Delay(Timeout.Infinite);
    }
}