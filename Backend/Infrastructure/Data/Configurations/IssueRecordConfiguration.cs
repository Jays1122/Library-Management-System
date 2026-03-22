using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configurations
{
    public class IssueRecordConfiguration : IEntityTypeConfiguration<IssueRecord>
    {
        public void Configure(EntityTypeBuilder<IssueRecord> builder)
        {
            builder.ToTable("IssueRecords");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.IssueDate).IsRequired();

            // Foreign Key Relations (Guid)
            builder.HasOne(i => i.Book)
                   .WithMany()
                   .HasForeignKey(i => i.BookId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.User)
                   .WithMany()
                   .HasForeignKey(i => i.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
