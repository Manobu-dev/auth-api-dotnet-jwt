using AuthApi.Data;
using AuthApi.DTOs.Auth;
using AuthApi.Models;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwt;

        public AuthController(AppDbContext context, JwtTokenService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Username == dto.Username);
            if (exists)
                return BadRequest("Usuário já existe");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Created("", "Usuário registrado com sucesso");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
                return Unauthorized("Usuário ou senha inválidos");

            var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!validPassword)
                return Unauthorized("Usuário ou senha inválidos");

            var tokenString = _jwt.GenerateToken(user.Id, user.Username);

            return Ok(new
            {
                message = "Login realizado com sucesso",
                token = tokenString,
                tokenType = "Bearer"
            });
        }
    }
}