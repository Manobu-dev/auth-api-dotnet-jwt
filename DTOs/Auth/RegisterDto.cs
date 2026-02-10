using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}