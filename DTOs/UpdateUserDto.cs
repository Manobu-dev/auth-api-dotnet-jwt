using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;
    }
}