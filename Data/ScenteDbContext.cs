using Microsoft.EntityFrameworkCore;
using Scente.API.Entity;

namespace Scente.API.Data;

public class ScenteDbContext : DbContext
{
    public ScenteDbContext(DbContextOptions<ScenteDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVolume> ProductVolumes => Set<ProductVolume>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Cart> Carts => Set<Cart>();

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Product config
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Price).HasColumnType("decimal(10,2)");
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.Property(p => p.Brand).IsRequired().HasMaxLength(100);
            e.HasMany(p => p.Volumes)
             .WithOne(v => v.Product)
             .HasForeignKey(v => v.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ProductVolume config
        modelBuilder.Entity<ProductVolume>(e =>
        {
            e.HasKey(v => v.Id);
            e.Property(v => v.Price).HasColumnType("decimal(10,2)");
            e.Property(v => v.Size).HasMaxLength(20);
        });

        // User config
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).IsRequired().HasMaxLength(200);
        });

        // Cart config
        modelBuilder.Entity<Cart>(e =>
        {
            e.HasKey(c => c.Id);
            e.HasOne(c => c.User)
             .WithMany()
             .HasForeignKey(c => c.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        }); 

        // Wishlist config
        modelBuilder.Entity<Wishlist>(e =>
        {
            e.HasKey(w => w.Id);
            e.HasOne(w => w.User)
             .WithMany()
             .HasForeignKey(w => w.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Order config
        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(o => o.Id);
            e.HasOne(o => o.User)
             .WithMany()
             .HasForeignKey(o => o.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        SeedData.Seed(modelBuilder);
    }
}