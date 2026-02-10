
using AuthApi.Data;
using AuthApi.Models;
using AuthApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new { user.Id, user.Username });
        }
        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/users/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/users
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id },
                new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username
                });
        }

        // PUT: api/users/1
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.Username = dto.Username;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/users/1
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}