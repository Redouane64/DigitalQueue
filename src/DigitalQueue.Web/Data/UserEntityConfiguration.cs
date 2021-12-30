using DigitalQueue.Web.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalQueue.Web.Data;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Ignore(e => e.EmailConfirmed);
        builder.Ignore(e => e.LockoutEnabled);
        builder.Ignore(e => e.LockoutEnd);
        builder.Ignore(e => e.PhoneNumber);
        builder.Ignore(e => e.AccessFailedCount);
        builder.Ignore(e => e.TwoFactorEnabled);
        builder.Ignore(e => e.PhoneNumberConfirmed);
        builder.Ignore(e => e.ConcurrencyStamp);
        builder.Ignore(e => e.SecurityStamp);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Email).HasColumnName("email");
        builder.Property(e => e.UserName).HasColumnName("username");

        builder.Property(e => e.NormalizedEmail).HasColumnName("normalized_email");
        builder.Property(e => e.NormalizedUserName).HasColumnName("normalized_username");
        builder.Property(e => e.PasswordHash).HasColumnName("password_hash");
    }
}