using DigitalQueue.Web.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalQueue.Web.Data;

public class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");
        
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Title).HasColumnName("title");
        
        builder.HasMany(
            e => e.Members
        ).WithMany(
            e => e.Courses
        ).UsingEntity<Membership>(
            e => e.HasOne(m => m.User)
                .WithMany(m => m.CoursesMemberships)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict),
            e => e.HasOne(c => c.Course)
                .WithMany(c => c.CourseMemberships)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Restrict),
            m => m.HasKey(t => new {t.CourseId, t.UserId})
        );
    }
}

public class MembershipEntityConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("memberships");
        
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.CourseId).HasColumnName("course_id");
        builder.Property(e => e.IsTeacher).HasColumnName("is_teacher");
    }
}
