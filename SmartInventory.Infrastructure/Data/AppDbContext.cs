using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;

namespace SmartInventory.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<RefreshToken>().HasOne(x => x.ApplicationUser).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.ApplicationUserId);
        builder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Product>().HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Product>().Property(x => x.Price).HasPrecision(18, 2);
        builder.Entity<Supplier>().HasQueryFilter(x => !x.IsDeleted);

        builder.Entity<Purchase>().HasOne(x => x.Supplier).WithMany(x => x.Purchases).HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<PurchaseItem>().HasOne(x => x.Purchase).WithMany(x => x.PurchaseItems).HasForeignKey(x => x.PurchaseId);
        builder.Entity<PurchaseItem>().HasOne(x => x.Product).WithMany(x => x.PurchaseItems).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Customer>().HasQueryFilter(x => !x.IsDeleted);

        builder.Entity<Sale>().HasOne(x => x.Customer).WithMany(x => x.Sales).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<SaleItem>().HasOne(x => x.Sale).WithMany(x => x.SaleItems).HasForeignKey(x => x.SaleId);
        builder.Entity<SaleItem>().HasOne(x => x.Product).WithMany(x => x.SaleItems).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
    }
}