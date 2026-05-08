namespace Scente.API.Entity;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string ProductName { get; set; } = string.Empty;  // snapshot at purchase time
    public decimal Price { get; set; }                        // snapshot at purchase time
    public int Quantity { get; set; }
    public string Size { get; set; } = string.Empty;          // e.g. "50ml"
}