namespace Scente.API.Entity;

public class ProductVolume
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Size { get; set; } = string.Empty;   // "30ml", "50ml", "100ml"
    public decimal Price { get; set; }

    public Product Product { get; set; } = null!;
}