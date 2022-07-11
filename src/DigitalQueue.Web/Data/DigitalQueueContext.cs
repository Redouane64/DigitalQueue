using DigitalQueue.Web.Areas.Courses.Models;
using DigitalQueue.Web.Data.Common;
using DigitalQueue.Web.Data.Courses;
using DigitalQueue.Web.Data.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Data;

public class DigitalQueueContext : IdentityDbContext<ApplicationUser>
{
    public DigitalQueueContext(DbContextOptions<DigitalQueueContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        builder.ApplyConfiguration(new RoleEntityConfiguration());

        builder.Ignore<IdentityUserToken<string>>();
        builder.Ignore<IdentityUserLogin<string>>();
        builder.Ignore<IdentityRoleClaim<string>>();

        builder.Entity<IdentityUserClaim<string>>().ToTable("users_claims");
        builder.Entity<IdentityUserRole<string>>().ToTable("users_roles");

        builder.ApplyConfiguration(new CourseEntityConfiguration());
        builder.ApplyConfiguration(new QueueItemEntityConfiguration());
        builder.ApplyConfiguration(new SessionEntityConfiguration());

        builder.Entity<CourseQueueItem>().HasNoKey().ToTable(name: null);
    }

    public override int SaveChanges()
    {
        SetCreateAtAndUpdatedAtFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        SetCreateAtAndUpdatedAtFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetCreateAtAndUpdatedAtFields()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IBaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((IBaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                ((IBaseEntity)entityEntry.Entity).CreateAt = DateTime.Now;
            }
        }
    }

    public DbSet<Course> Courses { get; set; }

    public DbSet<QueueItem> Queues { get; set; }

    public DbSet<Session> Sessions { get; set; }
}
