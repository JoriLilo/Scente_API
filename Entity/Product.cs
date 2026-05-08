namespace Scente.API.Entity;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;   // "Niche" | "Luxury"
    public string Gender { get; set; } = string.Empty;     // "Unisex" | "Men" | "Women"
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Image { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "active";

    // Fragrance notes (stored as comma-separated strings)
    public string? TopNotes { get; set; }
    public string? MiddleNotes { get; set; }
    public string? BaseNotes { get; set; }

    // Navigation: a product can have many volumes/prices
    public ICollection<ProductVolume> Volumes { get; set; } = new List<ProductVolume>();
}