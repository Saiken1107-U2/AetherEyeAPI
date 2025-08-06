using System.ComponentModel.DataAnnotations;

namespace AetherEyeAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public required string NombreCompleto { get; set; }

        [Required]
        public required string Correo { get; set; }

        [Required]
        public required string Contrasena { get; set; }

        public int RolId { get; set; }
        public Rol? Rol { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool EstaActivo { get; set; } = true;
    }
}
