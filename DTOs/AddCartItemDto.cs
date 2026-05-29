namespace Scente.API.DTOs;

public class AddCartItemDto
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public string Size { get; set; } = string.Empty;
    public decimal Price { get; set; }
}