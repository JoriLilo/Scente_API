using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.Entity;
using Scente.API.DTOs;

namespace Scente.API.Controllers;

[ApiController] //tells ASP.NET Core that this class is an API controller.
[Route("api/admin")]//means all your endpoints start with /api/admin

public class AdminController : ControllerBase
{
  //readonly means that the variable can only be assigned once, either at the point of declaration or in the constructor. This ensures that the database context is not accidentally changed after it has been set. 
  private readonly ScenteDbContext _db; //A private variable to hold the database.

  public AdminController(ScenteDbContext db)
  {
    _db = db;
  }

  [HttpGet("products")] // products is added to the full url
  //async and await means the method waits for the database without freezing the server. 

  //IActionresult means the method returns an HTTP response (Ok,NotFound)
  public async Task<IActionResult> GetProducts([FromQuery]int page = 1, [FromQuery] int pageSize = 20)
    {
      //counts total nr of products in DB
      var total = await _db.Products.CountAsync();
      var products = await _db.Products
        .Skip((page - 1) * pageSize).OrderBy(p => p.Id) //skips products from previous page
        .Take(pageSize) // takes only the 20 products for the current page
        .ToListAsync();//the moment where u send the query to the database and get the results back as a list of products
        return Ok(new {data = products, total, page, pageSize});// sends back a 200 success response with the data
    }

  [HttpPost("products")]
  
  public async Task<IActionResult> CreateProduct(CreateProductDto dto)
  {
    var product = new Product
    {
      Name = dto.Name,
      Brand = dto.Brand,
      Category = dto.Category,
      Gender = dto.Gender,
      Price = dto.Price,
      Stock = dto.Stock,
      Image = dto.Image,
      Description = dto.Description,
      TopNotes = dto.TopNotes,
      MiddleNotes = dto.MiddleNotes,
      BaseNotes = dto.BaseNotes,
      Status = "active"
    };

    _db.Products.Add(product);
    await _db.SaveChangesAsync();

    return Ok(product);
  }

  [HttpPut("products/{id}")]
  public async Task<IActionResult> UpdateProduct(int id, CreateProductDto dto)
  {
    var product = await _db.Products.FindAsync(id);
    if(product == null)
    {
      return NotFound(new {message = "Product not found"});
    }
    
    product.Name = dto.Name;
    product.Brand = dto.Brand;
    product.Category = dto.Category;
    product.Gender = dto.Gender;
    product.Price = dto.Price;
    product.Stock = dto.Stock;
    product.Image = dto.Image;
    product.Description = dto.Description;
    product.TopNotes = dto.TopNotes;
    product.MiddleNotes = dto.MiddleNotes;
    product.BaseNotes = dto.BaseNotes;

    await _db.SaveChangesAsync();
    return Ok(product);
  }
   
  [HttpDelete("products/{id}")]

  public async Task<IActionResult> DeleteProduct(int id)
  {
    var product = await _db.Products.FindAsync(id);

    if(product == null)
    {
      return NotFound(new {message = "Product not found"});
    }

    _db.Products.Remove(product);
    await _db.SaveChangesAsync();
    return Ok();
  }


[HttpGet("stats")]
public async Task<IActionResult> GetStats()
  {
    var products = await _db.Products.CountAsync();
    var orders = await _db.Orders.CountAsync();
    var users = await _db.Users.CountAsync();
    var revenue = await _db.Orders.SumAsync(o => o.TotalPaid);

    return Ok(new {products,orders,users,revenue});
  }
}