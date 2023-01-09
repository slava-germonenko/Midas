using Microsoft.EntityFrameworkCore;
using Midas.Common.HostedServices;
using Midas.Core;

namespace Midas.Auth.Core;

public class CleanupSessionsJob : ITimedHostedService
{
    private const int BatchSize = 100;

    private readonly MidasContext _context;

    public CleanupSessionsJob(MidasContext context)
    {
        _context = context;
    }

    public async Task RunOnceAsync(CancellationToken cancellationToken = new())
    {
        var expiredSessions = await _context.Sessions
            .Where(s => s.ExpireDate <= DateTime.UtcNow)
            .ToListAsync(cancellationToken: cancellationToken);

        _context.Sessions.RemoveRange(expiredSessions);
        await _context.SaveChangesAsync(cancellationToken);
    }
}