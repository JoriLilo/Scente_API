namespace Scente.API.Entity;

public class Wishlist
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation: a wishlist can have multiple items
     public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
}