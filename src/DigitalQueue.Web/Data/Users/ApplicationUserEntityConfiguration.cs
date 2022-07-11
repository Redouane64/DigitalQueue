using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalQueue.Web.Data.Users;

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("users");

        builder.Ignore(e => e.LockoutEnabled);
        builder.Ignore(e => e.LockoutEnd);
        builder.Ignore(e => e.PhoneNumber);
        builder.Ignore(e => e.AccessFailedCount);
        builder.Ignore(e => e.TwoFactorEnabled);
        builder.Ignore(e => e.PhoneNumberConfirmed);
        builder.Ignore(e => e.EmailConfirmed);
        builder.Ignore(e => e.ConcurrencyStamp);

        builder.Property(e => e.SecurityStamp).HasColumnName("security_stamp");

        builder.Property(e => e.UserName).HasColumnName("username");
        builder.Property(e => e.NormalizedUserName).HasColumnName("normalized_username");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Email).HasColumnName("email");

        builder.Property(e => e.NormalizedEmail).HasColumnName("normalized_email");
        builder.Property(e => e.PasswordHash).HasColumnName("password_hash");

        builder.Property(e => e.Name).HasColumnName("name");

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasMany(e => e.Sessions)
            .WithOne(e => e.User)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
