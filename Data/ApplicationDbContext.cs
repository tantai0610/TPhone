using CellPhoneS.Common;
using CellPhoneS.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneS.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderDetail>()
                    .HasKey(e => new { e.OrderId, e.Id, e.ProductId });

        modelBuilder.Entity<Orders>()
                    .Property(e => e.Status)
                    .HasDefaultValue(true);


        modelBuilder.Entity<Product>()
                    .Property(e => e.Price)
                    .HasDefaultValue(0);
        modelBuilder.Entity<Product>()
                   .Property(e => e.PromotionPrice)
                   .HasDefaultValue(0);
        modelBuilder.Entity<Product>()
                   .Property(e => e.Rating)
                   .HasDefaultValue(0);
        modelBuilder.Entity<Product>()
                   .Property(e => e.Quantity)
                   .HasDefaultValue(0);
        modelBuilder.Entity<Product>()
                   .Property(e => e.IsHot)
                   .HasDefaultValue(false);
        modelBuilder.Entity<Product>()
                   .Property(e => e.ViewCount)
                   .HasDefaultValue(0);
        modelBuilder.Entity<Product>()
                   .HasIndex(e => e.SeoName)
                   .IsUnique();

        modelBuilder.Entity<ProductCategory>()
                   .Property(e => e.Status)
                   .HasDefaultValue(true);
        modelBuilder.Entity<ProductCategory>()
                   .HasIndex(e => e.SeoName)
                   .IsUnique();



        // Add Relationship
        modelBuilder.Entity<ProductCategory>()
                    .HasMany(e => e.Products)
                    .WithOne(e => e.ProductCategory)
                    .HasForeignKey(e => e.ProductCategoryId)
                    .IsRequired();

        modelBuilder.Entity<Product>()
                    .HasOne(e => e.ProductCategory)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.ProductCategoryId)
                    .IsRequired();

        modelBuilder.Entity<Brand>()
                    .HasMany(e => e.Products)
                    .WithOne(e => e.Brand)
                    .HasForeignKey(e => e.BrandId)
                    .IsRequired();

        modelBuilder.Entity<Product>()
                    .HasOne(e => e.Brand)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.BrandId)
                    .IsRequired();

        modelBuilder.Entity<Supplier>()
                    .HasMany(e => e.Products)
                    .WithOne(e => e.Supplier)
                    .HasForeignKey(e => e.SupplierId)
                    .IsRequired();

        modelBuilder.Entity<Product>()
                    .HasOne(e => e.Supplier)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.SupplierId)
                    .IsRequired();

        modelBuilder.Entity<Orders>()
                    .HasMany(e => e.OrderDetails)
                    .WithOne(e => e.Orders)
                    .HasForeignKey(e => e.OrderId)
                    .IsRequired();

        modelBuilder.Entity<OrderDetail>()
                    .HasOne(e => e.Orders)
                    .WithMany(e => e.OrderDetails)
                    .HasForeignKey(e => e.OrderId)
                    .IsRequired();

        modelBuilder.Entity<Product>()
                    .HasMany(e => e.OrderDetails)
                    .WithOne(e => e.Product)
                    .HasForeignKey(e => e.ProductId)
                    .IsRequired();

        modelBuilder.Entity<OrderDetail>()
                    .HasOne(e => e.Product)
                    .WithMany(e => e.OrderDetails)
                    .HasForeignKey(e => e.ProductId)
                    .IsRequired();
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseModel && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                ((BaseModel)entityEntry.Entity).CreatedAt = DateTime.Now;
            }
            ((BaseModel)entityEntry.Entity).UpdatedAt = DateTime.Now;
        }

        return base.SaveChanges();
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
}