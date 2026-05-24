using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.DTOs;
using Scente.API.Entity;
using System.Security.Claims;

namespace Scente.API.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize] // must be logged in — placing an order while logged out returns 401
public class OrdersController : ControllerBase
{
    private readonly ScenteDbContext _db;

    public OrdersController(ScenteDbContext db)
    {
        _db = db;
    }

    // =========================================================
    // POST /api/orders
    // Creates an Order + all OrderItems from the user's current
    // cart, clears the cart, and returns the generated order
    // number. Status defaults to "pending".
    // =========================================================
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        var userId = GetUserId();

        // 1. Load the user's cart together with its items and the
        //    real Product behind each item (we need the DB price).
        var cart = await _db.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        // 2. Guard: no cart, or an empty cart, cannot become an order.
        if (cart == null || cart.Items.Count == 0)
        {
            return BadRequest(new
            {
                message = "Your cart is empty."
            });
        }

        // 3. Build the order. The total is summed from the cart
        //    items here on the server — we never trust a total
        //    sent from the browser.
        var order = new Order
        {
            OrderNumber    = GenerateOrderNumber(),
            UserId         = userId,
            Date           = DateTime.UtcNow,
            Status         = "pending",          // default on creation
            PaymentMethod  = dto.PaymentMethod,
            TotalPaid      = cart.Items.Sum(i => i.Price * i.Quantity),

            // Shipping snapshot — saved onto the order so it stays
            // correct even if the user changes their address later.
            ShippingAddress = dto.ShippingAddress,
            City            = dto.City,
            PostalCode      = dto.PostalCode,
            Country         = dto.Country,
            Phone           = dto.Phone
        };

        // 4. Copy every cart item into an order item. We snapshot
        //    the name and price at purchase time so order history
        //    is always accurate even if the product changes later.
        order.Items = cart.Items.Select(i => new OrderItem
        {
            ProductId   = i.ProductId,
            ProductName = i.Product.Name,
            Price       = i.Price,
            Quantity    = i.Quantity,
            Size        = i.Size
        }).ToList();

        // 5. Save the order, then empty the cart.
        _db.Orders.Add(order);
        _db.CartItems.RemoveRange(cart.Items);
        await _db.SaveChangesAsync();

        // 6. Return the order number for the confirmation modal.
        return Ok(new
        {
            orderNumber = order.OrderNumber,
            totalPaid   = order.TotalPaid,
            status      = order.Status
        });
    }

    // =========================================================
    // Generates an order code: one letter + 14 digits.
    // (Same shape the old frontend used, e.g. "A12345678901234")
    // =========================================================
    private static string GenerateOrderNumber()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();

        var letter = letters[random.Next(letters.Length)];

        var digits = "";
        for (var i = 0; i < 14; i++)
        {
            digits += random.Next(10);
        }

        return letter + digits;
    }

    // =========================================================
    // Extract current user ID from JWT token
    // (identical pattern to WishlistController)
    // =========================================================
    private int GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.Parse(userId!);
    }
}
