using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Data;

public class DigitalQueueContext : IdentityDbContext<User>
{
    public DigitalQueueContext(DbContextOptions<DigitalQueueContext> options)
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserEntityConfiguration());
        builder.ApplyConfiguration(new RoleEntityConfiguration());

        builder.Ignore<IdentityUserToken<string>>();
        builder.Ignore<IdentityUserLogin<string>>();
        builder.Ignore<IdentityRoleClaim<string>>();

        builder.Entity<IdentityUserClaim<string>>().ToTable("users_claims");
        builder.Entity<IdentityUserRole<string>>().ToTable("users_roles");

        builder.ApplyConfiguration(new CourseEntityConfiguration());
        builder.ApplyConfiguration(new MembershipEntityConfiguration());
    }

    public DbSet<Course> Courses { get; set; }
    
}
