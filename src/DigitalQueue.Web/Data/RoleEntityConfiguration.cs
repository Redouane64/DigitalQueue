using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalQueue.Web.Data;

public class RoleEntityConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.ToTable("roles");

        builder.Ignore(e => e.ConcurrencyStamp);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.NormalizedName).HasColumnName("normalized_name");

        builder.HasData(new List<IdentityRole>()
        {
            new IdentityRole(RoleDefaults.Administrator) { NormalizedName = RoleDefaults.Administrator.ToUpper() },
            new IdentityRole(RoleDefaults.User) { NormalizedName = RoleDefaults.User.ToUpper() },
        });
    }
}
