using LearnASP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnASP.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Book Entity Configuration
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                // Primary Key
                entity.HasKey(book => book.Id);

                // Properties
                entity.Property(book => book.Title)
                      .IsRequired()
                      .HasMaxLength(200);
                
                //entity.Property(book => book.Author)
                //        .IsRequired()
                //        .HasMaxLength(100);
                
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
                        .HasDefaultValueSql("GETUTCDATE()");

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

                // Foreign Key Relationship many to one with Author
                entity.HasOne(book => book.Author)
                        .WithMany(author => author.Books)
                        .HasForeignKey(book => book.AuthorId);

            });

            // Author Entity Configuration
            modelBuilder.Entity<Author>(author =>
            {
                author.ToTable("Authors");

                author.HasKey(author => author.Id);

                author.Property(author => author.FullName)
                      .IsRequired()
                      .HasMaxLength(100);

                author.Property(author => author.Biography)
                      .HasMaxLength(2000);

                author.Property(author => author.Nationality)
                      .HasMaxLength(100);

                author.Property(author => author.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                author.Property(author => author.CreatedBy)
                      .HasDefaultValue(1);

                author.Property(author => author.UpdatedBy)
                      .HasDefaultValue(1);

                author.Property(author => author.IsDeleted)
                      .HasDefaultValue(false);

                author.HasQueryFilter(author => !author.IsDeleted);

                // Foreign Key Relationship one to many with Book
                author.HasMany(author => author.Books)
                      .WithOne(book => book.Author)
                      .HasForeignKey(book => book.AuthorId);
            });
        }

    }
}
