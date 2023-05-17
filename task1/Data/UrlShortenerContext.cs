using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using task1.Models;

namespace task1.Data
{
    public class UrlShortenerContext : IdentityDbContext
    {
        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : base(options) { }

        public DbSet<IdentityUser> User { get; set; }
        public DbSet<Url> Url { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>
            (entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.PasswordHash).HasMaxLength(50).HasAnnotation("Minlength", 6);

            });


            modelBuilder.Entity<Url>()
          .HasOne(e => e.User)
          .WithMany(e => e.Urls)
          .HasForeignKey(e => e.UserId)
          .HasConstraintName("FK_Url_User_UserId");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.Property("IsDeleted").CurrentValue = true;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
