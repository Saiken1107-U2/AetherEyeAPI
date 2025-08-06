namespace AetherEyeAPI.Models
{
    public class VentaProductoRequest
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class VentaRequest
    {
        public int UsuarioId { get; set; }
        public required List<VentaProductoRequest> Productos { get; set; }
    }
}