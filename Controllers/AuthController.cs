using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scente.API.Data;
using Scente.API.DTOs;
using Scente.API.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Scente.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ScenteDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(
        ScenteDbContext db,
        IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existingUser = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (existingUser != null)
        {
            return BadRequest(new
            {
                message = "Email already exists"
            });
        }

        var hashedPassword =
            BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = hashedPassword,
            JoinDate = DateTime.UtcNow,
            Role = "customer"
        };

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        var token = GenerateJwt(user);

        return Ok(new
        {
            token,
            name = user.FirstName,
            email = user.Email
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null ||
            !BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash))
        {
            return Unauthorized(new
            {
                message = "Invalid credentials"
            });
        }

        var token = GenerateJwt(user);

        return Ok(new
        {
            token,
            name = user.FirstName,
            email = user.Email
        });
    }

    private string GenerateJwt(User user)
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Name,
                user.FirstName),

            new Claim(
                ClaimTypes.Email,
                user.Email),

            new Claim(
                ClaimTypes.Role,
                user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _config["Jwt:Key"]!));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}