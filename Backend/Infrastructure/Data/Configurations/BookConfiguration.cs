using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(b => b.Id); // Id BaseEntity se aa rahi hai

            builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
            builder.Property(b => b.Author).IsRequired().HasMaxLength(150);
            builder.Property(b => b.ISBN).HasMaxLength(50);
        }
    }
}
