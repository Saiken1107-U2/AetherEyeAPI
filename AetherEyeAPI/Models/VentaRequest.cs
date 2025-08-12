using System.ComponentModel.DataAnnotations;

namespace AetherEyeAPI.Models
{
    public class VentaRequest
    {
        [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre del cliente no puede exceder 100 caracteres")]
        public required string NombreCliente { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        [StringLength(100, ErrorMessage = "El correo no puede exceder 100 caracteres")]
        public string? CorreoCliente { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? TelefonoCliente { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        public string? DireccionCliente { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        [RegularExpression("^(Efectivo|Tarjeta|Transferencia)$", ErrorMessage = "El método de pago debe ser Efectivo, Tarjeta o Transferencia")]
        public required string MetodoPago { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El descuento debe ser mayor o igual a 0")]
        public decimal Descuento { get; set; } = 0;

        [Required(ErrorMessage = "Debe incluir al menos un producto")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un producto")]
        public required List<DetalleVentaRequest> Productos { get; set; }
    }

    public class DetalleVentaRequest
    {
        [Required(ErrorMessage = "El ID del producto es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del producto debe ser mayor a 0")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public decimal Cantidad { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0")]
        public decimal? PrecioUnitario { get; set; } // Opcional, si no se proporciona se toma del producto
    }

    public class ActualizarEstadoVentaRequest
    {
        [Required(ErrorMessage = "El estado es obligatorio")]
        [RegularExpression("^(Pendiente|Procesando|Enviado|Entregado|Cancelado)$", 
            ErrorMessage = "El estado debe ser: Pendiente, Procesando, Enviado, Entregado o Cancelado")]
        public required string Estado { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }
    }

    public class VentaResponse
    {
        public int Id { get; set; }
        public string? NombreCliente { get; set; }
        public string? CorreoCliente { get; set; }
        public string? TelefonoCliente { get; set; }
        public string? DireccionCliente { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? MetodoPago { get; set; }
        public string? Observaciones { get; set; }
        public string? NumeroFactura { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public string? VendedorNombre { get; set; }
        public List<DetalleVentaResponse> Productos { get; set; } = new List<DetalleVentaResponse>();
    }

    public class DetalleVentaResponse
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string? ProductoNombre { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    // Mantener compatibilidad con el modelo anterior
    public class VentaProductoRequest
    {
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class EstadisticasVentasResponse
    {
        public int VentasHoy { get; set; }
        public int VentasSemana { get; set; }
        public int VentasMes { get; set; }
        public decimal IngresosHoy { get; set; }
        public decimal IngresosSemana { get; set; }
        public decimal IngresosMes { get; set; }
        public List<VentasPorEstado> VentasPorEstado { get; set; } = new();
        public List<ProductoMasVendido> ProductosMasVendidos { get; set; } = new();
    }

    public class VentasPorEstado
    {
        public string Estado { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class ProductoMasVendido
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal IngresoTotal { get; set; }
    }
}