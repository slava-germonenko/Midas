namespace Midas.Common.HostedServices;

public class TimedServiceConfiguration<TService> where TService : ITimedHostedService
{
    /// <summary>
    /// Time span between run intervals.
    /// </summary>
    public TimeSpan SleepSpan { get; set; }

    /// <summary>
    /// Identifies whether service should run first time immediately
    /// or wait for <see cref="SleepSpan"/> at first.
    /// </summary>
    public bool StartImmediately { get; set; } = true;
}