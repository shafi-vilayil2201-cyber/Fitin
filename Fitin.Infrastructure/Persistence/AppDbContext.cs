using Fitin.Domain.Entities;
using Fitin.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Fitin.Domain.Entities.CartItems;
using Fitin.Domain.Entities.Wishlists;
using CloudinaryDotNet.Actions;
using Fitin.Domain.Entities.Categories;
using Fitin.Domain.Entities.Supplements;
// using Fitin.Domain.Common;
// using System.Linq.Expressions;

namespace Fitin.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Supplement> Supplements => Set<Supplement>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(x => x.Id); //define primary key

                builder.HasIndex(x => x.Email)
                    .HasFilter("[IsDeleted] = 0")
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

                builder.Property(x => x.Stock)
                    .IsRequired();

                builder.Property(x => x.ImageUrl)
                    .IsRequired()
                    .HasDefaultValue(string.Empty);

                builder.Property(x => x.Brand)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValue(string.Empty);

                builder.Property(x => x.Sport)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValue(string.Empty);

                builder.Property(x => x.Description)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDefaultValue(string.Empty);

                builder.Property(x => x.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValue(string.Empty);

                builder.Property(x => x.LongDescription)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasDefaultValue(string.Empty);

                builder.Property(x => x.Rating)
                    .HasPrecision(4, 2)
                    .HasDefaultValue(0m);

                builder.Property(x => x.Discount)
                    .HasPrecision(5, 2)
                    .HasDefaultValue(0m);

                builder.Property(x => x.Price)
                    .HasPrecision(18, 2);
                
                builder.HasOne(x=> x.Category)
                    .WithMany()
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Supplement>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                builder.Property(x => x.Stock)
                    .IsRequired();

                builder.Property(x => x.ImageUrl)
                    .IsRequired()
                    .HasDefaultValue(string.Empty);

                builder.Property(x => x.Brand)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValue(string.Empty);

                builder.Property(x => x.Description)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDefaultValue(string.Empty);

                builder.Property(x => x.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValue(string.Empty);

                builder.Property(x => x.LongDescription)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasDefaultValue(string.Empty);

                builder.Property(x => x.Rating)
                    .HasPrecision(4, 2)
                    .HasDefaultValue(0m);

                builder.Property(x => x.Discount)
                    .HasPrecision(5, 2)
                    .HasDefaultValue(0m);

                builder.Property(x => x.Price)
                    .HasPrecision(18, 2);
                
                builder.HasOne(x => x.Category)
                    .WithMany()
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
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

                builder.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(x => new {x.UserId, x.ProductId})
                    .HasFilter("[IsDeleted] = 0")
                    .IsUnique();
            });

            modelBuilder.Entity<WishlistItem>(builder =>
            {
                builder.HasKey(x => x.Id); 

                builder.HasOne<User>()
                    .WithMany("WishlistItems")
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                builder.HasOne(x=> x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                builder.HasIndex(x => new {x.UserId,x.ProductId} )
                    .HasFilter("[IsDeleted] = 0")
                    .IsUnique();
                
            });
            
            modelBuilder.Entity<Order>(builder=>
            {
                builder.HasKey(x=>x.Id);

                builder.Property(x=>x.TotalAmount)
                    .HasPrecision(18,2);
                
                builder.Property(x=> x.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.HasOne<User>()
                    .WithMany("Orders")
                    .HasForeignKey(x=>x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(builder =>
            {
                builder.HasKey(x=>x.Id);

                builder.Property(x => x.ProductName)
                    .IsRequired()
                    .HasMaxLength(200);
                
                builder.Property(x=>x.UnitPrice)
                    .HasPrecision(18,2);
                
                builder.Property(x=> x.Quantity)
                    .IsRequired();

                builder.HasOne(x => x.Order)
                    .WithMany(x=> x.OrderItems)
                    .HasForeignKey(x=> x.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x=> x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Category>(builder =>
            {
                builder.HasKey(x=> x.Id);

                builder.Property(x=>x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.HasIndex(x=> x.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Product>()
            .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Supplement>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<CartItem>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<WishlistItem>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<User>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Order>()
                .HasQueryFilter(x => !x.IsDeleted);
            
            modelBuilder.Entity<OrderItem>()
                .HasQueryFilter(x => !x.IsDeleted);
            // ApplySoftDeleteQueryFilters(modelBuilder);

        }

        // private static void ApplySoftDeleteQueryFilters(ModelBuilder modelBuilder)
        // {
        //     foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        //     {
        //         if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
        //             continue;

        //         var parameter = Expression.Parameter(entityType.ClrType, "e");
        //         var isDeletedProperty = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
        //         var filter = Expression.Lambda(Expression.Equal(isDeletedProperty, Expression.Constant(false)), parameter);

        //         entityType.SetQueryFilter(filter);
        //     }
        // }
    }
}
