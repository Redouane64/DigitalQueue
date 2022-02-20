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
            e => e.Teachers
        ).WithMany(
            e => e.TeacherOf
        ).UsingEntity(e =>
        {
            e.ToTable("course_teacher");
            e.Property("TeacherOfId").HasColumnName("course_id");
            e.Property("TeachersId").HasColumnName("teacher_id");
        });
        
    }
}

