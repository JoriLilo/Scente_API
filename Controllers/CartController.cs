using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.Entity;
using System.Security.Claims;

namespace Scente.API.Controllers;

[ApiController]
[Route("api/cart")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ScenteDbContext _db;

    public CartController(ScenteDbContext db)
    {
        _db = db;
    }

    // =========================================================
    // GET /api/cart
    // Returns current user's cart with full product data
    // =========================================================
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetUserId();

        var cart = await _db.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            return Ok(new { items = new List<object>(), subtotal = 0 });
        }

        var items = cart.Items.Select(i => new
        {
            id      = i.Id,
            name    = i.Product.Name,
            brand   = i.Product.Brand,
            price   = i.Price,
            qty     = i.Quantity,
            image   = i.Product.Image,
            size    = i.Size,
            productId = i.ProductId
        }).ToList();

        var subtotal = items.Sum(i => i.price * i.qty);

        return Ok(new { items, subtotal });
    }

    // =========================================================
    // GET /api/cart/count
    // Returns total item count for navbar badge
    // =========================================================
    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        var userId = GetUserId();

        var cart = await _db.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        var count = cart?.Items.Sum(i => i.Quantity) ?? 0;

        return Ok(new { count });
    }

    // =========================================================
    // POST /api/cart/items
    // Add item to cart (duplicate product+size increments qty)
    // =========================================================
    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemDto dto)
    {
        var userId = GetUserId();

        // Validate product exists and has stock
        var product = await _db.Products.FindAsync(dto.ProductId);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        if (product.Stock <= 0)
            return BadRequest(new { message = "Out of stock" });

        // Get or create cart
        var cart = await _db.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync();
        }

        // Check if same product+size already in cart
        var existing = cart.Items.FirstOrDefault(i =>
            i.ProductId == dto.ProductId && i.Size == dto.Size);

        if (existing != null)
        {
            existing.Quantity += 1;
        }
        else
        {
            // Determine price: use volume price if available, else product base price
            decimal price = product.Price;
            if (!string.IsNullOrEmpty(dto.Size))
            {
                var volume = await _db.ProductVolumes
                    .FirstOrDefaultAsync(v => v.ProductId == dto.ProductId && v.Size == dto.Size);
                if (volume != null) price = volume.Price;
            }

            cart.Items.Add(new CartItem
            {
                CartId    = cart.Id,
                ProductId = dto.ProductId,
                Quantity  = 1,
                Size      = dto.Size ?? "50ml",
                Price     = price
            });
        }

        await _db.SaveChangesAsync();

        return Ok(new { message = "Added to cart" });
    }

    // =========================================================
    // PATCH /api/cart/items/{id}
    // Update quantity of a cart item
    // =========================================================
    [HttpPatch("items/{id}")]
    public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateCartItemDto dto)
    {
        var userId = GetUserId();

        var cart = await _db.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return NotFound(new { message = "Cart not found" });

        var item = cart.Items.FirstOrDefault(i => i.Id == id);
        if (item == null) return NotFound(new { message = "Item not found" });

        if (dto.Quantity <= 0)
        {
            _db.CartItems.Remove(item);
        }
        else
        {
            item.Quantity = dto.Quantity;
        }

        await _db.SaveChangesAsync();
        return Ok(new { message = "Updated" });
    }

    // =========================================================
    // DELETE /api/cart/items/{id}
    // Remove a single item from cart
    // =========================================================
    [HttpDelete("items/{id}")]
    public async Task<IActionResult> RemoveItem(int id)
    {
        var userId = GetUserId();

        var cart = await _db.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return NotFound(new { message = "Cart not found" });

        var item = cart.Items.FirstOrDefault(i => i.Id == id);
        if (item == null) return NotFound(new { message = "Item not found" });

        _db.CartItems.Remove(item);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Removed" });
    }

    // =========================================================
    // DELETE /api/cart
    // Clear entire cart (called after order is placed)
    // =========================================================
    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        var userId = GetUserId();

        var cart = await _db.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return Ok(new { message = "Cart already empty" });

        _db.CartItems.RemoveRange(cart.Items);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Cart cleared" });
    }

    // =========================================================
    // POST /api/cart/promo
    // Validate a promo code
    // =========================================================
    [HttpPost("promo")]
    public async Task<IActionResult> ValidatePromo([FromBody] PromoDto dto)
    {
        var promo = await _db.PromoCodes
            .FirstOrDefaultAsync(p =>
                p.Code == dto.Code.ToUpper() &&
                p.IsActive &&
                (p.ExpiresAt == null || p.ExpiresAt > DateTime.UtcNow));

        if (promo == null)
            return BadRequest(new { message = "Invalid or expired promo code" });

        return Ok(new
        {
            code         = promo.Code,
            discountRate = promo.DiscountRate,
            message      = $"{(int)(promo.DiscountRate * 100)}% discount applied!"
        });
    }

    // =========================================================
    // Helper — extract user ID from JWT
    // =========================================================
    private int GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userId!);
    }
}

// ── DTOs ──────────────────────────────────────────────────
public class AddCartItemDto
{
    public int    ProductId { get; set; }
    public string? Size     { get; set; }
}

public class UpdateCartItemDto
{
    public int Quantity { get; set; }
}

public class PromoDto
{
    public string Code { get; set; } = string.Empty;
}