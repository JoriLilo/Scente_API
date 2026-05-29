using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.Entity;
using System.Security.Claims;

namespace Scente.API.Controllers;

[ApiController]
[Route("api/products/{productId}/reviews")]
public class ReviewController : ControllerBase
{
    private readonly ScenteDbContext _db;

    public ReviewController(ScenteDbContext db) => _db = db;

    // GET /api/products/{productId}/reviews
    // Returns all reviews + average star rating for a product
    [HttpGet]
    public async Task<IActionResult> GetReviews(int productId)
    {
        var reviews = await _db.Reviews
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new
            {
                r.Id,
                r.AuthorName,
                r.Rating,
                r.Text,
                r.CreatedAt
            })
            .ToListAsync();

        var average = reviews.Count > 0
            ? Math.Round(reviews.Average(r => r.Rating), 1)
            : 0.0;

        return Ok(new { reviews, average, count = reviews.Count });
    }

    // POST /api/products/{productId}/reviews
    // Saves a new review — requires login
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddReview(int productId, [FromBody] CreateReviewDto dto)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _db.Users.FindAsync(userId);

        var review = new Review
        {
            ProductId  = productId,
            UserId     = userId,
            AuthorName = $"{user!.FirstName} {user.LastName}".Trim(),
            Rating     = Math.Clamp(dto.Rating, 1, 5),
            Text       = dto.Text.Trim(),
            CreatedAt  = DateTime.UtcNow
        };

        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Review submitted", review.Id });
    }
}

public class CreateReviewDto
{
    public int Rating { get; set; }
    public string Text { get; set; } = string.Empty;
}
