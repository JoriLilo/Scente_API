using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scente.API.Data;
using Scente.API.DTOs;
using Scente.API.Entity;
using System.Security.Claims;

namespace Scente.API.Controllers;

// WEEK 1 — Kristi (Profile)
[ApiController]
[Route("api/users")]
[Authorize] // every route here needs a valid JWT
public class UsersController : ControllerBase
{
    private readonly ScenteDbContext _db;

    public UsersController(ScenteDbContext db)
    {
        _db = db;
    }

    // Same pattern the other controllers use
    private int GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userId!);
    }

    // =========================================================
    // GET /api/users/me
    // Logged-in user's profile. Password is NEVER returned.
    // =========================================================
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetUserId();

        var user = await _db.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            user.JoinDate
        });
    }

    // =========================================================
    // PUT /api/users/me
    // Updates first + last name.
    // =========================================================
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName) ||
            string.IsNullOrWhiteSpace(dto.LastName))
        {
            return BadRequest(new { message = "First and last name are required" });
        }

        var userId = GetUserId();

        var user = await _db.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        user.FirstName = dto.FirstName.Trim();
        user.LastName = dto.LastName.Trim();
        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Profile updated",
            user.FirstName,
            user.LastName,
            user.Email,
            user.JoinDate
        });
    }

    // =========================================================
    // PUT /api/users/me/password
    // Verifies the old password, hashes + saves the new one.
    // =========================================================
    [HttpPut("me/password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.OldPassword) ||
            string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            return BadRequest(new { message = "Both old and new passwords are required" });
        }

        if (dto.NewPassword.Length < 6)
        {
            return BadRequest(new { message = "New password must be at least 6 characters" });
        }

        var userId = GetUserId();

        var user = await _db.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
        {
            return BadRequest(new { message = "Current password is incorrect" });
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Password updated successfully" });
    }
}

