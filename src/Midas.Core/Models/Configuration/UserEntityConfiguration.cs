using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Midas.Core.Models.Configuration;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasAlternateKey(user => user.Pesel);
        builder.HasAlternateKey(user => user.Email);
        builder.HasAlternateKey(user => user.Phone);

        builder.HasMany(u => u.DynamicProperties)
            .WithOne()
            .HasForeignKey("UserId");
    }
}