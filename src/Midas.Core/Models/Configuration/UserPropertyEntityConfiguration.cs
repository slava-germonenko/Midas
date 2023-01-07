using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Midas.Core.Models.Configuration;

public class UserPropertyEntityConfiguration : IEntityTypeConfiguration<UserProperty>
{
    public void Configure(EntityTypeBuilder<UserProperty> builder)
    {
        builder.HasKey(up => new { up.Name, up.UserId });
    }
}