using Microsoft.EntityFrameworkCore;
using Obscura.Models;

namespace LearnASP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Book> Books => Set<Book>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(book => book.Id);
                entity.Property(book => book.Title)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(book => book.Author)
                        .IsRequired()
                        .HasMaxLength(100);
                entity.Property(book => book.Description)
                        .HasMaxLength(2000);
                entity.Property(book => book.Publisher)
                        .IsRequired()
                        .HasMaxLength(100);
                entity.Property(book => book.Price)
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");
                entity.Property(book => book.CreatedAt)
                        .IsRequired()
                        .HasDefaultValue("GETUTCDATE");
                entity.Property(book => book.UpdatedAt);
                entity.Property(book => book.CreatedBy)
                        .IsRequired()
                        .HasDefaultValue(1);    // default to admin - change it later
                entity.Property(book => book.UpdatedBy)
                        .IsRequired()
                        .HasDefaultValue(1);    // default to admin - change it later
                entity.Property(book => book.IsDeleted)
                        .IsRequired();
                entity.Property(book => book.IsAvailable)
                        .IsRequired();
                entity.Property(book => book.IsForSale)
                        .IsRequired();
            });
        }

    }
}
