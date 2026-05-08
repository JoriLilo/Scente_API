namespace Scente.API.Entity;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "pending";       // pending | shipped | delivered
    public string PaymentMethod { get; set; } = string.Empty; // "card" | "cod"
    public decimal TotalPaid { get; set; }

    // Shipping snapshot
    public string ShippingAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // Navigation: order items (replaces the wrong ICollection<Product>)
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}