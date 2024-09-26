using System.ComponentModel.DataAnnotations;

namespace PeliculasApiRestFul.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Foto { get; set; }
    }
}
