namespace AetherEyeAPI.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Completada, Cancelada
        public string? NumeroFactura { get; set; }
        public string? Observaciones { get; set; }
        
        // Relación con DetalleCompras
        public virtual ICollection<DetalleCompra> DetallesCompra { get; set; } = new List<DetalleCompra>();
    }
}
