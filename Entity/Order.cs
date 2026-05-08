namespace Scente.API.Entity;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "pending";   

    // Navigation: an order can have multiple items
    public ICollection<Product> Items { get; set; } = new List<Product>();

}