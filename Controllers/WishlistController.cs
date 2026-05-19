using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.Entity;
using System.Security.Claims;

namespace Scente.API.Controllers;

[ApiController]
[Route("api/wishlist")]
[Authorize]
public class WishlistController : ControllerBase
{
    private readonly ScenteDbContext _db;

    public WishlistController(ScenteDbContext db)
    {
        _db = db;
    }

    // =========================================================
    // GET /api/wishlist
    // Returns current user's wishlist with full product data
    // =========================================================
    [HttpGet]
    public async Task<IActionResult> GetWishlist()
    {
        var userId = GetUserId();

        var wishlist = await _db.Wishlists
            .Include(w => w.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wishlist == null)
        {
            return Ok(new List<Product>());
        }

        var products = wishlist.Items
            .Select(i => i.Product)
            .ToList();

        return Ok(products);
    }

    // =========================================================
    // POST /api/wishlist/{productId}
    // Adds product to wishlist
    // =========================================================
    [HttpPost("{productId}")]
    public async Task<IActionResult> AddToWishlist(int productId)
    {
        var userId = GetUserId();

        var product = await _db.Products.FindAsync(productId);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Product not found"
            });
        }

        var wishlist = await _db.Wishlists
            .Include(w => w.Items)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        // Create wishlist if user doesn't have one
        if (wishlist == null)
        {
            wishlist = new Wishlist
            {
                UserId = userId
            };

            _db.Wishlists.Add(wishlist);

            await _db.SaveChangesAsync();
        }

        // Prevent duplicates
        var alreadyExists = wishlist.Items
            .Any(i => i.ProductId == productId);

        if (alreadyExists)
        {
            return BadRequest(new
            {
                message = "Product already in wishlist"
            });
        }

        var item = new WishlistItem
        {
            WishlistId = wishlist.Id,
            ProductId = productId
        };

        _db.WishlistItems.Add(item);

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Added to wishlist"
        });
    }

    // =========================================================
    // DELETE /api/wishlist/{productId}
    // Removes product from wishlist
    // =========================================================
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveFromWishlist(int productId)
    {
        var userId = GetUserId();

        var wishlist = await _db.Wishlists
            .Include(w => w.Items)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wishlist == null)
        {
            return NotFound(new
            {
                message = "Wishlist not found"
            });
        }

        var item = wishlist.Items
            .FirstOrDefault(i => i.ProductId == productId);

        if (item == null)
        {
            return NotFound(new
            {
                message = "Item not found in wishlist"
            });
        }

        _db.WishlistItems.Remove(item);

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Removed from wishlist"
        });
    }

    // =========================================================
    // Extract current user ID from JWT token
    // =========================================================
    private int GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.Parse(userId!);
    }
}