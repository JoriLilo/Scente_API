using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.DTOs;
using Scente.API.Entity;
using Scente.API.Services;
using System.Security.Claims;

namespace Scente.API.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize] // must be logged in -- placing an order while logged out returns 401
public class OrdersController : ControllerBase
{
    private readonly ScenteDbContext _db;
    private readonly IEmailService _email;

    // ---- Shipping rules (server-side, Week 2) ----------------
    private const decimal FreeShippingThreshold = 50m;
    private const decimal FlatShippingCost      = 15m;

    public OrdersController(ScenteDbContext db, IEmailService email)
    {
        _db = db;
        _email = email;
    }

    // =========================================================
    // POST /api/orders
    // Creates an Order + OrderItems from the user's DB cart,
    // calculates shipping + total server-side, validates the
    // address, clears the cart, sends a confirmation email, and
    // returns the generated order number.
    // =========================================================
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        var userId = GetUserId();

        // 1. Week 3 — server-side address validation. Trim guards
        //    against whitespace-only values that [Required] allows.
        if (string.IsNullOrWhiteSpace(dto.City) ||
            string.IsNullOrWhiteSpace(dto.Country) ||
            string.IsNullOrWhiteSpace(dto.Phone) ||
            string.IsNullOrWhiteSpace(dto.ShippingAddress))
        {
            return BadRequest(new
            {
                message = "Shipping address, city, country and phone are required."
            });
        }

        // 2. Load the user's cart with items + their real products.
        var cart = await _db.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        // 3. Guard: an empty/missing cart returns a clear 400.
        if (cart == null || cart.Items.Count == 0)
        {
            return BadRequest(new { message = "Your cart is empty." });
        }

        // 4. SERVER-SIDE money math (never trusts the client).
        var subtotal = cart.Items.Sum(i => i.Price * i.Quantity);
        var shipping = CalculateShipping(subtotal);
        var total    = subtotal + shipping;

        // 5. Build the order.
        var order = new Order
        {
            OrderNumber   = GenerateOrderNumber(),
            UserId        = userId,
            Date          = DateTime.UtcNow,
            Status        = "pending",
            PaymentMethod = dto.PaymentMethod,
            TotalPaid     = total,

            ShippingAddress = dto.ShippingAddress,
            City            = dto.City,
            PostalCode      = dto.PostalCode,
            Country         = dto.Country,
            Phone           = dto.Phone
        };

        order.Items = cart.Items.Select(i => new OrderItem
        {
            ProductId   = i.ProductId,
            ProductName = i.Product.Name,
            Price       = i.Price,
            Quantity    = i.Quantity,
            Size        = i.Size
        }).ToList();

        // 6. Save the order, then empty the cart (keep the cart row).
        _db.Orders.Add(order);
        _db.CartItems.RemoveRange(cart.Items);
        await _db.SaveChangesAsync();

        // 7. Week 3 — send the confirmation email. This runs AFTER
        //    the save, and a failure here never fails the order
        //    (the service swallows + logs its own errors).
        var user = await _db.Users.FindAsync(userId);
        if (user != null)
        {
            var emailData = new OrderEmailData
            {
                ToEmail           = user.Email,
                CustomerName      = user.FirstName,
                OrderNumber       = order.OrderNumber,
                Subtotal          = subtotal,
                ShippingCost      = shipping,
                TotalPaid         = total,
                EstimatedDelivery = order.Date.AddDays(5).ToString("MMM d, yyyy"),
                Items = order.Items.Select(i => new OrderEmailLine
                {
                    ProductName = i.ProductName,
                    Size        = i.Size,
                    Quantity    = i.Quantity,
                    Price       = i.Price
                }).ToList()
            };

            await _email.SendOrderConfirmationAsync(emailData);
        }

        // 8. Return the essentials for the confirmation modal.
        return Ok(new
        {
            orderNumber  = order.OrderNumber,
            subtotal     = subtotal,
            shippingCost = shipping,
            totalPaid    = order.TotalPaid,
            status       = order.Status
        });
    }

    // =========================================================
    // GET /api/orders/{orderNumber}/confirmation
    // Authoritative order summary for the thank-you modal.
    // =========================================================
    [HttpGet("{orderNumber}/confirmation")]
    public async Task<IActionResult> GetConfirmation(string orderNumber)
    {
        var userId = GetUserId();

        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o =>
                o.OrderNumber == orderNumber && o.UserId == userId);

        if (order == null)
        {
            return NotFound(new { message = "Order not found." });
        }

        var subtotal = order.Items.Sum(i => i.Price * i.Quantity);
        var shipping = order.TotalPaid - subtotal;

        return Ok(new
        {
            orderNumber       = order.OrderNumber,
            status            = order.Status,
            date              = order.Date,
            paymentMethod     = order.PaymentMethod,
            subtotal          = subtotal,
            shippingCost      = shipping,
            totalPaid         = order.TotalPaid,
            estimatedDelivery = order.Date.AddDays(5).ToString("MMM d, yyyy"),
            items = order.Items.Select(i => new
            {
                name     = i.ProductName,
                price    = i.Price,
                quantity = i.Quantity,
                size     = i.Size
            })
        });
    }

    // =========================================================
    private static decimal CalculateShipping(decimal subtotal)
    {
        return subtotal >= FreeShippingThreshold ? 0m : FlatShippingCost;
    }

    private static string GenerateOrderNumber()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();
        var letter = letters[random.Next(letters.Length)];
        var digits = "";
        for (var i = 0; i < 14; i++) digits += random.Next(10);
        return letter + digits;
    }

    private int GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userId!);
    }
}