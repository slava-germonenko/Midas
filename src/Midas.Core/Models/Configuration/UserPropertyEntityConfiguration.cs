using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Midas.Core.Models.Configuration;

public class UserPropertyEntityConfiguration : IEntityTypeConfiguration<UserProperty>
{
    public void Configure(EntityTypeBuilder<UserProperty> builder)
    {
        builder.Property<int>("UserId");
        builder.HasKey("Name", "UserId");
    }
}