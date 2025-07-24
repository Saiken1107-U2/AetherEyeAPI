namespace AetherEyeAPI.Models
{
    public class Cotizacion
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Pendiente";
    }
}
