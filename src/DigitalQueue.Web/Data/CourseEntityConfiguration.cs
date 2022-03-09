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
        builder.Property(e => e.Year).HasColumnName("year");
        builder.Property(e => e.IsArchived)
            .HasColumnName("is_archived")
            .HasDefaultValue(false);

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");
        
        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");
        
        builder.HasMany(
            e => e.Teachers
        ).WithMany(
            e => e.TeacherOf
        ).UsingEntity(e =>
        {
            e.ToTable("course_teacher");
            e.Property("TeacherOfId").HasColumnName("course_id");
            e.Property("TeachersId").HasColumnName("teacher_id");
        });

        builder.HasIndex(e => new { e.Title, e.Year }).IsUnique();
    }
}

