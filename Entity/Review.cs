namespace Scente.API.Entity;

public class Review
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string AuthorName { get; set; } = string.Empty;
    public int Rating { get; set; }          // 1–5
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}