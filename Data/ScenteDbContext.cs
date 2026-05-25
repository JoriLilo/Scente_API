using Microsoft.EntityFrameworkCore;
using Scente.API.Entity;

namespace Scente.API.Data;

public class ScenteDbContext : DbContext
{
    public ScenteDbContext(DbContextOptions<ScenteDbContext> options) : base(options) { }

    public DbSet<Product>      Products      => Set<Product>();
    public DbSet<ProductVolume> ProductVolumes => Set<ProductVolume>();
    public DbSet<User>         Users         => Set<User>();
    public DbSet<Order>        Orders        => Set<Order>();
    public DbSet<OrderItem>    OrderItems    => Set<OrderItem>();
    public DbSet<Cart>         Carts         => Set<Cart>();
    public DbSet<CartItem>     CartItems     => Set<CartItem>();
    public DbSet<Wishlist>     Wishlists     => Set<Wishlist>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
    public DbSet<Review>       Reviews       => Set<Review>();
    public DbSet<PromoCode>    PromoCodes    => Set<PromoCode>();
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── Product ──────────────────────────────────────────────
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

        // ── ProductVolume ─────────────────────────────────────────
        modelBuilder.Entity<ProductVolume>(e =>
        {
            e.HasKey(v => v.Id);
            e.Property(v => v.Price).HasColumnType("decimal(10,2)");
            e.Property(v => v.Size).HasMaxLength(20);
        });

        // ── User ──────────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).IsRequired().HasMaxLength(200);
            e.Property(u => u.FirstName).HasMaxLength(100);
            e.Property(u => u.LastName).HasMaxLength(100);
            e.Property(u => u.Role).HasMaxLength(50).HasDefaultValue("customer");
        });

        // ── Order ─────────────────────────────────────────────────
        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(o => o.Id);
            e.Property(o => o.TotalPaid).HasColumnType("decimal(10,2)");
            e.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
            e.HasIndex(o => o.OrderNumber).IsUnique();
            e.HasOne(o => o.User)
             .WithMany()
             .HasForeignKey(o => o.UserId)
             .OnDelete(DeleteBehavior.Restrict); // don't delete orders if user is deleted
            e.HasMany(o => o.Items)
             .WithOne(i => i.Order)
             .HasForeignKey(i => i.OrderId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── OrderItem ─────────────────────────────────────────────
        modelBuilder.Entity<OrderItem>(e =>
        {
            e.HasKey(i => i.Id);
            e.Property(i => i.Price).HasColumnType("decimal(10,2)");
            e.Property(i => i.ProductName).HasMaxLength(200);
            e.Property(i => i.Size).HasMaxLength(20);
            e.HasOne(i => i.Product)
             .WithMany()
             .HasForeignKey(i => i.ProductId)
             .OnDelete(DeleteBehavior.Restrict); // keep order history even if product deleted
        });

        // ── Cart ──────────────────────────────────────────────────
        modelBuilder.Entity<Cart>(e =>
        {
            e.HasKey(c => c.Id);
            e.HasOne(c => c.User)
             .WithMany()
             .HasForeignKey(c => c.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasMany(c => c.Items)
             .WithOne(i => i.Cart)
             .HasForeignKey(i => i.CartId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── CartItem ──────────────────────────────────────────────
        modelBuilder.Entity<CartItem>(e =>
        {
            e.HasKey(i => i.Id);
            e.Property(i => i.Price).HasColumnType("decimal(10,2)");
            e.Property(i => i.Size).HasMaxLength(20);
            e.HasOne(i => i.Product)
             .WithMany()
             .HasForeignKey(i => i.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Wishlist ──────────────────────────────────────────────
        modelBuilder.Entity<Wishlist>(e =>
        {
            e.HasKey(w => w.Id);
            e.HasOne(w => w.User)
             .WithMany()
             .HasForeignKey(w => w.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasMany(w => w.Items)
             .WithOne(i => i.Wishlist)
             .HasForeignKey(i => i.WishlistId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── WishlistItem ──────────────────────────────────────────
        modelBuilder.Entity<WishlistItem>(e =>
        {
            e.HasKey(i => i.Id);
            // Prevent duplicate products in the same wishlist
            e.HasIndex(i => new { i.WishlistId, i.ProductId }).IsUnique();
            e.HasOne(i => i.Product)
             .WithMany()
             .HasForeignKey(i => i.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Review ────────────────────────────────────────────────
        modelBuilder.Entity<Review>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.AuthorName).HasMaxLength(200);
            e.HasOne(r => r.Product)
             .WithMany()
             .HasForeignKey(r => r.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(r => r.User)
             .WithMany()
             .HasForeignKey(r => r.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── PromoCode ─────────────────────────────────────────────
        modelBuilder.Entity<PromoCode>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Code).IsRequired().HasMaxLength(50);
            e.HasIndex(p => p.Code).IsUnique();
            e.Property(p => p.DiscountRate).HasColumnType("decimal(5,2)");
        });
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // ── Seed data ─────────────────────────────────────────────
        SeedData.Seed(modelBuilder);  
    } 
}

