using Scente.API.Entity;
namespace Scente.API.Entity;

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public string Size { get; set; } = string.Empty;   // e.g. "50ml"
    public decimal Price { get; set; }                  // price at time of adding to cart
}