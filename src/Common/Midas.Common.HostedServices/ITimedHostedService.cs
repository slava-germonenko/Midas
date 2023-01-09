namespace Midas.Common.HostedServices;

public interface ITimedHostedService
{
    public Task RunOnceAsync(CancellationToken cancellationToken = new());
}