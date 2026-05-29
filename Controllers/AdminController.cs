using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.Entity;
using Scente.API.DTOs;

namespace Scente.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly ScenteDbContext _db;

    public AdminController(ScenteDbContext db)
    {
        _db = db;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var total = await _db.Products.CountAsync();
        var products = await _db.Products
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return Ok(new { data = products, total, page, pageSize });
    }

    [HttpPost("products")]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {
        var product = new Product
        {
            Name        = dto.Name,
            Brand       = dto.Brand,
            Category    = dto.Category,
            Gender      = dto.Gender,
            Price       = dto.Price,
            Stock       = dto.Stock,
            Image       = dto.Image,
            Description = dto.Description,
            TopNotes    = dto.TopNotes,
            MiddleNotes = dto.MiddleNotes,
            BaseNotes   = dto.BaseNotes,
            Status      = "active"
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return Ok(product);
    }

    [HttpPut("products/{id}")]
    public async Task<IActionResult> UpdateProduct(int id, CreateProductDto dto)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        product.Name        = dto.Name;
        product.Brand       = dto.Brand;
        product.Category    = dto.Category;
        product.Gender      = dto.Gender;
        product.Price       = dto.Price;
        product.Stock       = dto.Stock;
        product.Image       = dto.Image;
        product.Description = dto.Description;
        product.TopNotes    = dto.TopNotes;
        product.MiddleNotes = dto.MiddleNotes;
        product.BaseNotes   = dto.BaseNotes;

        await _db.SaveChangesAsync();
        return Ok(product);
    }

    [HttpDelete("products/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var products = await _db.Products.CountAsync();
        var orders   = await _db.Orders.CountAsync();
        var users    = await _db.Users.CountAsync();
        var revenue  = await _db.Orders.SumAsync(o => o.TotalPaid);
        return Ok(new { products, orders, users, revenue });
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(o =>
                o.OrderNumber.ToLower().Contains(term) ||
                o.User.FirstName.ToLower().Contains(term) ||
                o.User.LastName.ToLower().Contains(term) ||
                o.User.Email.ToLower().Contains(term));
        }

        var total = await query.CountAsync();

        var orders = await query
            .OrderByDescending(o => o.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new
            {
                o.Id,
                o.OrderNumber,
                o.Date,
                o.Status,
                o.TotalPaid,
                o.PaymentMethod,
                customer  = $"{o.User.FirstName} {o.User.LastName}".Trim(),
                email     = o.User.Email,
                itemCount = o.Items.Count
            })
            .ToListAsync();

        return Ok(new { data = orders, total, page, pageSize });
    }

    [HttpPut("orders/{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto dto)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order == null)
            return NotFound(new { message = "Order not found" });

        order.Status = dto.Status;
        await _db.SaveChangesAsync();
        return Ok(new { message = "Status updated", order.Status });
    }

    [HttpGet("orders/{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound(new { message = "Order not found" });

        return Ok(new
        {
            order.Id,
            order.OrderNumber,
            order.Date,
            order.Status,
            order.TotalPaid,
            order.PaymentMethod,
            order.ShippingAddress,
            order.City,
            order.PostalCode,
            order.Country,
            order.Phone,
            customer = $"{order.User.FirstName} {order.User.LastName}".Trim(),
            email    = order.User.Email,
            items    = order.Items.Select(i => new
            {
                i.ProductName,
                i.Price,
                i.Quantity,
                i.Size
            })
        });
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _db.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(u =>
                u.FirstName.ToLower().Contains(term) ||
                u.LastName.ToLower().Contains(term) ||
                u.Email.ToLower().Contains(term));
        }

        var total = await query.CountAsync();

        var users = await query
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.JoinDate,
                u.Role
            })
            .ToListAsync();

        return Ok(new { data = users, total, page, pageSize });
    }
}

public class UpdateOrderStatusDto
{
    public string Status { get; set; } = string.Empty;
}