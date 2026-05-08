namespace Scente.API.Entity;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation: a cart can have multiple items
    public ICollection<Product> Items { get; set; } = new List<Product>();
}