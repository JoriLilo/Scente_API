using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.Entity;

namespace Scente.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ScenteDbContext _db;

    public ProductsController(ScenteDbContext db) => _db = db;

    // GET /api/products
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? gender,
        [FromQuery] string? brand,
        [FromQuery] string? category,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] string? search,
        [FromQuery] string? sort)
    {
        var query = _db.Products.Include(p => p.Volumes).AsQueryable();

        if (!string.IsNullOrEmpty(gender))
            query = query.Where(p => p.Gender.ToLower() == gender.ToLower());

        if (!string.IsNullOrEmpty(brand))
            query = query.Where(p => p.Brand.ToLower() == brand.ToLower());

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category.ToLower() == category.ToLower());

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(p =>
                p.Name.Contains(search) ||
                p.Brand.Contains(search) ||
                p.Description.Contains(search));

        query = sort switch
        {
            "price-asc"  => query.OrderBy(p => p.Price),
            "price-desc" => query.OrderByDescending(p => p.Price),
            "newest"     => query.OrderByDescending(p => p.Id),
            _            => query.OrderBy(p => p.Id)
        };

        var products = await query
            .Where(p => p.Status == "active")
            .ToListAsync();

        return Ok(products);
    }

    // GET /api/products/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _db.Products
            .Include(p => p.Volumes)
            .FirstOrDefaultAsync(p => p.Id == id);

        return product is null ? NotFound() : Ok(product);
    }

    // POST /api/products  (admin only — auth added later by another teammate)
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT /api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        _db.Entry(product).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null) return NotFound();
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
