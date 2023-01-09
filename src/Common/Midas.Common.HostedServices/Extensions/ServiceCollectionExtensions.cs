using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Midas.Common.HostedServices.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterTimedHostedService<TService>(
        this IServiceCollection services,
        TimeSpan sleepSpan
    ) where TService : class, ITimedHostedService
        => services.RegisterTimedHostedService<TService>(options => options.SleepSpan = sleepSpan);

    public static void RegisterTimedHostedService<TService>(
        this IServiceCollection services,
        Action<TimedServiceConfiguration<TService>> configure
    ) where TService : class, ITimedHostedService
    {
        var serviceConfiguration = new TimedServiceConfiguration<TService>();
        configure(serviceConfiguration);
        services.AddSingleton(serviceConfiguration);
        services.TryAddScoped<TService>();
        services.AddHostedService<TimeHostedServicesRunner<TService>>();
    }
}