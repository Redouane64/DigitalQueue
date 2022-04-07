using DigitalQueue.Web.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalQueue.Web.Data;

public class RequestEntityConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.ToTable("requests");
        
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.CourseId).HasColumnName("course_id");
        builder.Property(e => e.CreatorId).HasColumnName("creator_id");
        builder.Property(e => e.Completed)
            .HasColumnName("completed")
            .HasDefaultValue(false);
        
        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");
        
        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

    }
}
