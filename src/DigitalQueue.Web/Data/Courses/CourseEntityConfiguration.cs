using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalQueue.Web.Data.Courses;

public class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("title");
        builder.Property(e => e.Year).HasColumnName("year")
               .IsRequired(false);
        builder.Property(e => e.Group).HasColumnName("group");
        builder.Property(e => e.Deleted)
            .HasColumnName("deleted")
            .HasDefaultValue(false);

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasMany(
            e => e.Teachers
        ).WithMany(
            e => e.Courses
        ).UsingEntity(e => {
            e.ToTable("course_teacher");
            e.Property("CoursesId").HasColumnName("course_id");
            e.Property("TeachersId").HasColumnName("teacher_id");
        });

        builder.HasMany(e => e.QueueItems)
               .WithOne(e => e.Course)
               .HasForeignKey(e => e.CourseId);

        builder.HasIndex(e => new { e.Name, e.Group }).IsUnique();
    }
}

