using System.ComponentModel.DataAnnotations;

namespace AetherEyeAPI.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string Nombre { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string NombreContacto { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public required string Correo { get; set; }
        
        [Required]
        [StringLength(15)]
        public required string Telefono { get; set; }
        
        [Required]
        [StringLength(200)]
        public required string Direccion { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string Ciudad { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string Pais { get; set; }
        
        [StringLength(10)]
        public string? CodigoPostal { get; set; }
        
        [StringLength(200)]
        public string? SitioWeb { get; set; }
        
        [StringLength(500)]
        public string? Descripcion { get; set; }
        
        public bool EstaActivo { get; set; } = true;
        
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
    
    public class ProveedorRequest
    {
        [Required]
        [StringLength(100)]
        public required string Nombre { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string NombreContacto { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public required string Correo { get; set; }
        
        [Required]
        [StringLength(15)]
        public required string Telefono { get; set; }
        
        [Required]
        [StringLength(200)]
        public required string Direccion { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string Ciudad { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string Pais { get; set; }
        
        [StringLength(10)]
        public string? CodigoPostal { get; set; }
        
        [StringLength(200)]
        public string? SitioWeb { get; set; }
        
        [StringLength(500)]
        public string? Descripcion { get; set; }
    }
}
