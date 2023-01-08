using Microsoft.EntityFrameworkCore;
using Midas.Core.Models;
using Midas.Core.Models.Configuration;

namespace Midas.Core;

public class MidasContext : DbContext
{
    public DbSet<Session> Sessions => Set<Session>();

    public DbSet<User> Users => Set<User>();

    public DbSet<UserProperty> UserProperties => Set<UserProperty>();

    public MidasContext(DbContextOptions<MidasContext> context) : base(context)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserPropertyEntityConfiguration());
    }
}