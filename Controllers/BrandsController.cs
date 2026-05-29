using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;

namespace Scente.API.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandsController : ControllerBase
{
    private readonly ScenteDbContext _db;

    public BrandsController(ScenteDbContext db) => _db = db;

    // GET /api/brands
    // Returns distinct brand names for the shop sidebar filter
    [HttpGet]
    public async Task<IActionResult> GetBrands()
    {
        var brands = await _db.Products
            .Where(p => p.Status == "active")
            .Select(p => p.Brand)
            .Distinct()
            .OrderBy(b => b)
            .ToListAsync();

        return Ok(brands);
    }
}