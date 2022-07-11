using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalQueue.Web.Data.Users;

public class SessionEntityConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("sessions");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.CreateAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

        builder.Property(e => e.AccessToken)
            .IsRequired()
            .HasColumnName("access_token");
        builder.Property(e => e.RefreshToken)
            .IsRequired()
            .HasColumnName("refresh_token");

        builder.Property(e => e.DeviceToken).HasColumnName("device_token");
        builder.Property(e => e.SecurityStamp).HasColumnName("security_stamp");

        builder.Property(e => e.UserId).HasColumnName("user_id");

        builder.HasIndex(e => e.RefreshToken);
        builder.HasIndex(e => new { e.UserId, e.SecurityStamp });
    }
}
