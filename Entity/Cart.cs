namespace Scente.API.Entity;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation: cart items (replaces the wrong ICollection<Product>)
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}