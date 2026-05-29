using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.DTOs;
using Scente.API.Entity;
using System.Security.Claims;


namespace Scente.API.Controllers;

[ApiController]
[Route("api/cart")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ScenteDbContext _db;

    public CartController(ScenteDbContext db) { _db = db; }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetUserId();

        var cart = await _db.Carts
            .Include(c => c.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return Ok(new { items = new List<object>(), subtotal = 0, warnings = new List<string>() });

        // Stock validation warnings (from cart/ari)
        var warnings = cart.Items
            .Where(i => i.Quantity > i.Product.Stock)
            .Select(i => $"Only {i.Product.Stock} units of {i.Product.Name} left in stock.")
            .ToList();

        var items = cart.Items.Select(i => new
        {
            id        = i.Id,
            name      = i.Product.Name,
            brand     = i.Product.Brand,
            price     = i.Price,
            qty       = i.Quantity,
            image     = i.Product.Image,
            size      = i.Size,
            productId = i.ProductId
        }).ToList();

        var subtotal = items.Sum(i => i.price * i.qty);

        return Ok(new { items, subtotal, warnings });
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        var userId = GetUserId();

        // Efficient DB-level sum (from cart/ari)
        var count = await _db.CartItems
            .Where(i => i.Cart.UserId == userId)
            .SumAsync(i => i.Quantity);

        return Ok(new { count });
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemDto dto)
    {
        var userId = GetUserId();

        var product = await _db.Products.FindAsync(dto.ProductId);
        if (product == null) return NotFound(new { message = "Product not found" });
        if (product.Stock <= 0) return BadRequest(new { message = "Out of stock" });

        var cart = await _db.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync();
        }

        var existing = cart.Items.FirstOrDefault(i =>
            i.ProductId == dto.ProductId && i.Size == dto.Size);

        if (existing != null)
        {
            existing.Quantity += 1;
        }
        else
        {
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

        // Stock check on update (from cart/ari)
        var product = await _db.Products.FindAsync(item.ProductId);
        if (product != null && dto.Quantity > product.Stock)
            return BadRequest(new { message = $"Only {product.Stock} items available." });

        if (dto.Quantity <= 0)
            _db.CartItems.Remove(item);
        else
            item.Quantity = dto.Quantity;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Updated" });
    }

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

    // POST /api/cart/merge
// Called on login — merges guest localStorage cart into the user's DB cart
[HttpPost("merge")]
public async Task<IActionResult> MergeCart([FromBody] List<GuestCartItemDto> guestItems)
{
    if (guestItems == null || guestItems.Count == 0)
        return Ok(new { message = "Nothing to merge" });

    var userId = GetUserId();

    var cart = await _db.Carts
        .Include(c => c.Items)
        .FirstOrDefaultAsync(c => c.UserId == userId);

    if (cart == null)
    {
        cart = new Cart { UserId = userId };
        _db.Carts.Add(cart);
        await _db.SaveChangesAsync();
    }

    foreach (var guestItem in guestItems)
    {
        var product = await _db.Products.FindAsync(guestItem.ProductId);
        if (product == null || product.Stock <= 0) continue;

        var existing = cart.Items.FirstOrDefault(i =>
            i.ProductId == guestItem.ProductId &&
            i.Size == guestItem.Size);

        if (existing != null)
        {
            existing.Quantity += guestItem.Quantity;
        }
        else
        {
            decimal price = product.Price;
            if (!string.IsNullOrEmpty(guestItem.Size))
            {
                var volume = await _db.ProductVolumes
                    .FirstOrDefaultAsync(v =>
                        v.ProductId == guestItem.ProductId &&
                        v.Size == guestItem.Size);
                if (volume != null) price = volume.Price;
            }

            cart.Items.Add(new CartItem
            {
                CartId    = cart.Id,
                ProductId = guestItem.ProductId,
                Quantity  = guestItem.Quantity,
                Size      = guestItem.Size ?? "50ml",
                Price     = price
            });
        }
    }

    await _db.SaveChangesAsync();
    return Ok(new { message = "Cart merged" });
}
    private int GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userId!);
    }
}

