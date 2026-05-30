like this using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.Documents;
using Scente.API.DTOs;
using Scente.API.Entity;
using Scente.API.Services;
using System.Security.Claims;

namespace Scente.API.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ScenteDbContext _db;
    private readonly IEmailService _email;

    private const decimal FreeShippingThreshold = 50m;
    private const decimal FlatShippingCost      = 15m;

    public OrdersController(ScenteDbContext db, IEmailService email)
    {
        _db   = db;
        _email = email;
    }

    // POST /api/orders
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        var userId = GetUserId();

        if (string.IsNullOrWhiteSpace(dto.City)    ||
            string.IsNullOrWhiteSpace(dto.Country) ||
            string.IsNullOrWhiteSpace(dto.Phone)   ||
            string.IsNullOrWhiteSpace(dto.ShippingAddress))
        {
            return BadRequest(new { message = "Shipping address, city, country and phone are required." });
        }

        var cart = await _db.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || cart.Items.Count == 0)
            return BadRequest(new { message = "Your cart is empty." });

        var subtotal = cart.Items.Sum(i => i.Price * i.Quantity);
        var shipping = CalculateShipping(subtotal);
        var total    = subtotal + shipping;

        var order = new Order
        {
            OrderNumber     = GenerateOrderNumber(),
            UserId          = userId,
            Date            = DateTime.UtcNow,
            Status          = "pending",
            PaymentMethod   = dto.PaymentMethod,
            TotalPaid       = total,
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

        _db.Orders.Add(order);
        _db.CartItems.RemoveRange(cart.Items);
        await _db.SaveChangesAsync();

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

        return Ok(new
        {
            orderNumber  = order.OrderNumber,
            subtotal,
            shippingCost = shipping,
            totalPaid    = order.TotalPaid,
            status       = order.Status
        });
    }

    // GET /api/orders
    [HttpGet]
    public async Task<IActionResult> GetMyOrders(
        [FromQuery] string? status,
        [FromQuery] string? search)
    {
        var userId = GetUserId();

        var query = _db.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId);

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
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var userId = GetUserId();

        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound(new { message = "Order not found" });

        if (order.UserId != userId)
            return StatusCode(403, new { message = "You can only view your own orders" });

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

    // GET /api/orders/counts
    [HttpGet("counts")]
    public async Task<IActionResult> GetOrderCounts()
    {
        var userId = GetUserId();
        var mine   = _db.Orders.Where(o => o.UserId == userId);

        var counts = new OrderCountsDto
        {
            All       = await mine.CountAsync(),
            Pending   = await mine.CountAsync(o => o.Status.ToLower() == "pending"),
            Shipped   = await mine.CountAsync(o => o.Status.ToLower() == "shipped"),
            Delivered = await mine.CountAsync(o => o.Status.ToLower() == "delivered")
        };

        return Ok(counts);
    }

    // GET /api/orders/{orderNumber}/confirmation
    [HttpGet("{orderNumber}/confirmation")]
    public async Task<IActionResult> GetConfirmation(string orderNumber)
    {
        var userId = GetUserId();

        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o =>
                o.OrderNumber == orderNumber && o.UserId == userId);

        if (order == null)
            return NotFound(new { message = "Order not found." });

        var subtotal = order.Items.Sum(i => i.Price * i.Quantity);
        var shipping = order.TotalPaid - subtotal;

        return Ok(new
        {
            orderNumber       = order.OrderNumber,
            status            = order.Status,
            date              = order.Date,
            paymentMethod     = order.PaymentMethod,
            subtotal,
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

    // GET /api/orders/{id}/invoice
    [HttpGet("{id:int}/invoice")]
    public async Task<IActionResult> GetInvoice(int id)
    {
        var userId = GetUserId();

        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound(new { message = "Order not found" });

        if (order.UserId != userId)
            return StatusCode(403, new { message = "You can only download your own invoices" });

        var user     = await _db.Users.FindAsync(userId);
        var pdfBytes = InvoiceDocument.Generate(order, user!);

        return File(pdfBytes, "application/pdf", $"invoice-{order.OrderNumber}.pdf");
    }

    // ── Helpers ───────────────────────────────────────────────
    private static decimal CalculateShipping(decimal subtotal)
        => subtotal >= FreeShippingThreshold ? 0m : FlatShippingCost;

    private static string GenerateOrderNumber()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();
        var letter = letters[random.Next(letters.Length)];
        var digits = string.Concat(Enumerable.Range(0, 14).Select(_ => random.Next(10)));
        return letter + digits;
    }

    private int GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userId!);
    }
}