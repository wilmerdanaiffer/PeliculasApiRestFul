using System.ComponentModel.DataAnnotations;

namespace PeliculasApiRestFul.DTOs
{
    public class UserInfoDTO
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
