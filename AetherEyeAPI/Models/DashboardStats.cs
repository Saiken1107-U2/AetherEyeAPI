namespace AetherEyeAPI.Models
{
    public class DashboardStats
    {
        public int TotalInsumos { get; set; }
        public decimal ValorTotalInventario { get; set; }
        public int InsumosStockBajo { get; set; }
        public int MovimientosHoy { get; set; }
        public int TotalProveedores { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
    }
}
