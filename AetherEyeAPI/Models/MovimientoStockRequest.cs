using System.ComponentModel.DataAnnotations;

namespace AetherEyeAPI.Models
{
    public class MovimientoStockRequest
    {
        [Required(ErrorMessage = "El tipo de movimiento es obligatorio")]
        [RegularExpression("^(ENTRADA|SALIDA|AJUSTE)$", ErrorMessage = "El tipo de movimiento debe ser ENTRADA, SALIDA o AJUSTE")]
        public required string TipoMovimiento { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public decimal Cantidad { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El costo unitario debe ser mayor a 0")]
        public decimal? CostoUnitario { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio")]
        [StringLength(200, ErrorMessage = "El motivo no puede exceder 200 caracteres")]
        public required string Motivo { get; set; }

        [StringLength(100, ErrorMessage = "El documento no puede exceder 100 caracteres")]
        public string? Documento { get; set; }
    }
}
