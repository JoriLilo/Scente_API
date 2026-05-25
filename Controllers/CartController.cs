using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.DTOs;
using Scente.API.Entity;

namespace Scente.API.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ScenteDbContext _db;

    public CartController(ScenteDbContext db)
    {
        _db = db;
    }

    // GET /api/cart?userId=1
    // Returns the user's cart with all CartItems + Product data
    [HttpGet]
    public async Task<IActionResult> GetCart([FromQuery] int userId)
    {
        var cart = await _db.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return Ok(new { id = 0, userId, items = new List<CartItem>() });

        return Ok(cart);
    }

    // POST /api/cart/items
    // Adds item to cart — if same product+size exists, increment qty
    [HttpPost("items")]
    public async Task<IActionResult> AddItem(AddCartItemDto dto)
    {
        // Get or create the cart for this user
        var cart = await _db.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == dto.UserId)
            ?? new Cart { UserId = dto.UserId };

        if (cart.Id == 0)
            _db.Carts.Add(cart);

        // Check if same product+size already in cart
        var existing = cart.Items.FirstOrDefault(i =>
            i.ProductId == dto.ProductId && i.Size == dto.Size);

        if (existing != null)
        {
            // Increment quantity
            existing.Quantity++;
        }
        else
        {
            // Add new CartItem
            cart.Items.Add(new CartItem
            {
                ProductId = dto.ProductId,
                Quantity = 1,
                Size = dto.Size,
                Price = dto.Price
            });
        }

        await _db.SaveChangesAsync();
        return Ok(cart);
    }

    // DELETE /api/cart/items/{id}
    // Removes a CartItem from the cart
    [HttpDelete("items/{id}")]
    public async Task<IActionResult> RemoveItem(int id)
    {
        var item = await _db.CartItems.FindAsync(id);

        if (item == null)
            return NotFound(new { message = "Cart item not found" });

        _db.CartItems.Remove(item);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Item removed" });
    }

    // PATCH /api/cart/items/{id}
    // Updates quantity of a CartItem
    [HttpPatch("items/{id}")]
    public async Task<IActionResult> UpdateQuantity(int id, UpdateCartItemDto dto)
    {
        var item = await _db.CartItems.FindAsync(id);

        if (item == null)
            return NotFound(new { message = "Cart item not found" });

        if (dto.Quantity <= 0)
        {
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Item removed (quantity reached 0)" });
        }

        item.Quantity = dto.Quantity;
        await _db.SaveChangesAsync();
        return Ok(item);
    }
}