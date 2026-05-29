using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.Documents;   // <-- ADDED (Kristi) for the PDF invoice
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
    // POST /api/orders   (IDI — unchanged)
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
    // ===============  KRISTI'S ENDPOINTS BELOW  ==============
    // =========================================================

    // ---------- WEEK 2 ----------

    // GET /api/orders?status=&search=
    // Returns ONLY the logged-in user's orders.
    // Tab filter -> ?status=,  search bar -> ?search=
    [HttpGet]
    public async Task<IActionResult> GetMyOrders(
        [FromQuery] string? status,
        [FromQuery] string? search)
    {
        var userId = GetUserId();

        var query = _db.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId);

        // "all" or empty = no status filter
        if (!string.IsNullOrWhiteSpace(status) && status.ToLower() != "all")
        {
            var s = status.ToLower();
            query = query.Where(o => o.Status.ToLower() == s);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(o => o.OrderNumber.Contains(term));
        }

        var orders = await query
            .OrderByDescending(o => o.Date)
            .Select(o => new OrderSummaryDto
            {
                Id          = o.Id,
                OrderNumber = o.OrderNumber,
                Date        = o.Date,
                Status      = o.Status,
                TotalPaid   = o.TotalPaid,
                ItemCount   = o.Items.Count
            })
            .ToListAsync();

        return Ok(orders);
    }

    // GET /api/orders/{id}
    // Single order with its items. 403 if it's not yours.
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var userId = GetUserId();

        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound(new { message = "Order not found" });
        }

        // Security: you can only see your own orders
        if (order.UserId != userId)
        {
            return StatusCode(403, new { message = "You can only view your own orders" });
        }

        var dto = new OrderDetailDto
        {
            Id              = order.Id,
            OrderNumber     = order.OrderNumber,
            Date            = order.Date,
            Status          = order.Status,
            PaymentMethod   = order.PaymentMethod,
            TotalPaid       = order.TotalPaid,
            ShippingAddress = order.ShippingAddress,
            City            = order.City,
            PostalCode      = order.PostalCode,
            Country         = order.Country,
            Phone           = order.Phone,
            Items           = order.Items.Select(i => new OrderItemDto
            {
                ProductName = i.ProductName,
                Price       = i.Price,
                Quantity    = i.Quantity,
                Size        = i.Size
            }).ToList()
        };

        return Ok(dto);
    }

    // ---------- WEEK 3 ----------

    // GET /api/orders/counts
    // Tab badge numbers in one call.
    [HttpGet("counts")]
    public async Task<IActionResult> GetOrderCounts()
    {
        var userId = GetUserId();
        var mine = _db.Orders.Where(o => o.UserId == userId);

        var counts = new OrderCountsDto
        {
            All       = await mine.CountAsync(),
            Pending   = await mine.CountAsync(o => o.Status.ToLower() == "pending"),
            Shipped   = await mine.CountAsync(o => o.Status.ToLower() == "shipped"),
            Delivered = await mine.CountAsync(o => o.Status.ToLower() == "delivered")
        };

        return Ok(counts);
    }

    // GET /api/orders/{id}/invoice
    // PDF download (QuestPDF). 403 if it's not yours.
    [HttpGet("{id:int}/invoice")]
    public async Task<IActionResult> GetInvoice(int id)
    {
        var userId = GetUserId();

        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound(new { message = "Order not found" });
        }

        if (order.UserId != userId)
        {
            return StatusCode(403, new { message = "You can only download your own invoices" });
        }

        var user = await _db.Users.FindAsync(userId);
        var pdfBytes = InvoiceDocument.Generate(order, user!);

        return File(pdfBytes, "application/pdf", $"invoice-{order.OrderNumber}.pdf");
    }

    // =========================================================
    // ===============  SHARED HELPERS (IDI)  =================
    // =========================================================

    // Generates an order code: one letter + 14 digits.
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

    // Extract current user ID from the JWT token
    private int GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userId!);
    }
}
