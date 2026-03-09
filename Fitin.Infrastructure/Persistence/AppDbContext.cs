using Fitin.Domain.Entities;
using Fitin.Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Fitin.Domain.Entities.CartItem;

namespace Fitin.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartItem> CartItems => Set<CartItem>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.HasIndex(x => x.Email)
                    .IsUnique();

                builder.Property(x => x.Role)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<RefreshToken>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.HasOne<User>()
                    .WithMany("RefreshTokens")
                    .HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<Product>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                builder.Property(x => x.Category)
                    .IsRequired();

                builder.Property(x => x.Stock)
                    .IsRequired();

                builder.Property(x => x.ImageUrl)
                    .IsRequired()
                    .HasDefaultValue(string.Empty);
            });
            modelBuilder.Entity<CartItem>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.Quantity)
                    .IsRequired();

                builder.HasOne<User>()
                    .WithMany("CartItems")
                    .HasForeignKey(x=>x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(x => new {x.UserId, x.ProductId})
                    .IsUnique();
            });


            // modelBuilder.Entity<CartItem>(builder =>
            // {
            //     builder.HasKey(x => x.Id);

            //     builder.Property(x => x.Quantity)
            //         .IsRequired();

            //     builder.HasOne<User>()
            //         .WithMany("CartItems")
            //         .HasForeignKey(x => x.UserId)
            //         .OnDelete(DeleteBehavior.Cascade);

            //     builder.HasOne<Product>()
            //         .WithMany()
            //         .HasForeignKey(x => x.ProductId)
            //         .OnDelete(DeleteBehavior.Cascade);

            //     builder.HasIndex(x => new { x.UserId, x.ProductId })
            //         .IsUnique();
            // });
        }
    }
}
