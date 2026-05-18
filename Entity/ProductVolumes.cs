using System.Text.Json.Serialization;

namespace Scente.API.Entity;

public class ProductVolume
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Size { get; set; } = string.Empty;
    public decimal Price { get; set; }

    [JsonIgnore]
    public Product Product { get; set; } = null!;
}