using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.DTOs;
using Scente.API.Entity;

namespace Scente.API.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase {
    private readonly ScenteDbContext _db;

    private static readonly Dictionary<string, decimal> PromoCodes = new() {
        { "SCENTE10", 0.10m },
        { "SUMMER20", 0.20m },
        { "VIP30",    0.30m }
    };

    public CartController(ScenteDbContext db) { _db = db; }

    [HttpGet]
    public async Task<IActionResult> GetCart([FromQuery] int userId) {
        var cart = await _db.Carts
            .Include(c => c.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return Ok(new { id = 0, userId, items = new List<CartItem>() });

        // Week 3: Stock Validation Warning
        var warnings = cart.Items
            .Where(i => i.Quantity > i.Product.Stock)
            .Select(i => $"Only {i.Product.Stock} units of {i.Product.Name} left in stock.")
            .ToList();

        return Ok(new { cart, warnings });
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCount([FromQuery] int userId) {
        var count = await _db.CartItems
            .Where(i => i.Cart.UserId == userId)
            .SumAsync(i => i.Quantity);
        return Ok(new { count });
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem(AddCartItemDto dto) {
        var cart = await _db.Carts.Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == dto.UserId) ?? new Cart { UserId = dto.UserId };
        if (cart.Id == 0) _db.Carts.Add(cart);

        var product = await _db.Products.FindAsync(dto.ProductId);
        if (product == null) return NotFound("Product not found");
        if (product.Stock <= 0) return BadRequest("Product is out of stock");

        var existing = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId && i.Size == dto.Size);
        if (existing != null) { existing.Quantity++; }
        else {
            cart.Items.Add(new CartItem { ProductId = dto.ProductId, Quantity = 1, Size = dto.Size, Price = dto.Price });
        }
        await _db.SaveChangesAsync();
        return Ok(cart);
    }

    [HttpPatch("items/{id}")]
    public async Task<IActionResult> UpdateQuantity(int id, UpdateCartItemDto dto) {
        var item = await _db.CartItems.Include(i => i.Product).FirstOrDefaultAsync(x => x.Id == id);
        if (item == null) return NotFound();
        
        if (dto.Quantity > item.Product.Stock) 
            return BadRequest($"Only {item.Product.Stock} items available.");

        if (dto.Quantity <= 0) { _db.CartItems.Remove(item); }
        else { item.Quantity = dto.Quantity; }
        
        await _db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("items/{id}")]
    public async Task<IActionResult> RemoveItem(int id) {
        var item = await _db.CartItems.FindAsync(id);
        if (item == null) return NotFound();
        _db.CartItems.Remove(item);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Item removed" });
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart([FromQuery] int userId) {
        var items = await _db.CartItems.Where(i => i.Cart.UserId == userId).ToListAsync();
        _db.CartItems.RemoveRange(items);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Cart cleared" });
    }

    [HttpPost("promo")]
    public IActionResult ApplyPromo([FromBody] ApplyPromoDto dto) {
        var code = dto.Code?.ToUpper().Trim();
        if (PromoCodes.TryGetValue(code, out var discount)) {
            return Ok(new { valid = true, discountPercent = (int)(discount * 100), discountRate = discount });
        }
        return Ok(new { valid = false, message = "Invalid promo code" });
    }

    [HttpPost("merge")]
    public IActionResult MergeCart([FromBody] CartMergeDto dto) {
        return Ok(new { message = "Guest cart merged successfully" });
    }
}
