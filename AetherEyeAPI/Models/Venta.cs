namespace AetherEyeAPI.Models
{
    public class Venta
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        
        // Información del cliente
        public string? NombreCliente { get; set; }
        public string? CorreoCliente { get; set; }
        public string? TelefonoCliente { get; set; }
        public string? DireccionCliente { get; set; }
        
        // Información de la venta
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Procesando, Enviado, Entregado, Cancelado
        public string? MetodoPago { get; set; } // Efectivo, Tarjeta, Transferencia
        public string? Observaciones { get; set; }
        public string? NumeroFactura { get; set; }
        
        // Cálculos
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Descuento { get; set; }
        
        // Navegación
        public virtual ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
    }
}
