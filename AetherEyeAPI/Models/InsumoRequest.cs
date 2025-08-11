using System.ComponentModel.DataAnnotations;

namespace AetherEyeAPI.Models
{
    public class InsumoRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public required string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [StringLength(50, ErrorMessage = "El código interno no puede exceder 50 caracteres")]
        public string? CodigoInterno { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [StringLength(50, ErrorMessage = "La categoría no puede exceder 50 caracteres")]
        public required string Categoria { get; set; }

        [Required(ErrorMessage = "La unidad de medida es obligatoria")]
        [StringLength(20, ErrorMessage = "La unidad de medida no puede exceder 20 caracteres")]
        public required string UnidadMedida { get; set; }

        [Required(ErrorMessage = "El costo unitario es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El costo unitario debe ser mayor a 0")]
        public decimal CostoUnitario { get; set; }

        [Required(ErrorMessage = "El stock actual es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El stock actual no puede ser negativo")]
        public decimal StockActual { get; set; }

        [Required(ErrorMessage = "El stock mínimo es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo")]
        public decimal StockMinimo { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El stock máximo no puede ser negativo")]
        public decimal? StockMaximo { get; set; }

        public int? ProveedorId { get; set; }

        public DateTime? FechaVencimiento { get; set; }
    }
}
