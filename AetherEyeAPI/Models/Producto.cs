namespace AetherEyeAPI.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
        public decimal PrecioVenta { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
