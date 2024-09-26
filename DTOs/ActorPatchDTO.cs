﻿using System.ComponentModel.DataAnnotations;

namespace PeliculasApiRestFul.DTOs
{
    public class ActorPatchDTO
    {
        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
