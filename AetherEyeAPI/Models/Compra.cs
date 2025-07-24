namespace AetherEyeAPI.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
    }
}
