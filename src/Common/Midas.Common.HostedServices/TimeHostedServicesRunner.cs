using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Midas.Common.HostedServices;

public class TimeHostedServicesRunner<TService> : BackgroundService where TService : ITimedHostedService
{
    private readonly IServiceProvider _services;

    public TimeHostedServicesRunner(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var configuration = GetServiceConfiguration();
        if (!configuration.StartImmediately)
        {
            await Task.Delay(configuration.SleepSpan, stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunSafe(stoppingToken);
            await Task.Delay(configuration.SleepSpan, stoppingToken);
        }
    }

    private async Task RunSafe(CancellationToken stoppingToken)
    {
        using var scope = _services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<TService>();
        var logger = scope.ServiceProvider.GetService<ILogger<TService>>();
        try
        {
            await service.RunOnceAsync(stoppingToken);
        }
        catch(Exception e)
        {
            logger?.LogError(e, "Error occurred trying to execute {0}", typeof(TService).Name);
        }
    }

    private TimedServiceConfiguration<TService> GetServiceConfiguration()
    {
        using var scope = _services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<TimedServiceConfiguration<TService>>();
    }
}