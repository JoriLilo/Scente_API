//dto represent what admin is allowed in/out

namespace Scente.API.DTOs;
// used when you want to import this class in other classes
public class CreateProductDto
{
  public string Name { get; set; } = string.Empty; // if no value is sent it default to "" instead of null
  public string Brand { get; set; } = string.Empty;
  public string Category { get; set; } = string.Empty;
  public string Gender { get; set; } = string.Empty;
  public decimal Price { get; set; }
  public int Stock { get; set; }
  public string Image { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public string TopNotes { get; set; } = string.Empty;
  public string MiddleNotes { get; set; } = string.Empty;
  public string BaseNotes { get; set; } = string.Empty;
}