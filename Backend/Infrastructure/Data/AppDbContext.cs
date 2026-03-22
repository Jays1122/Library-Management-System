using Backend.Domain.Common;
using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Backend.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets for our Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<IssueRecord> IssueRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ye ek line hamari saari Configuration classes (UserConfiguration, etc.) ko khud dhoondh kar apply kar degi
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        // AUTOMATIC AUDIT LOGGING MAGIC (For CreatedAt, UpdatedAt)
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        // entry.Entity.CreatedBy = "System"; // Default "System" BaseEntity me set hai
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        // entry.Entity.UpdatedBy = "System";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
