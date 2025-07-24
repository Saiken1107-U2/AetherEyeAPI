namespace AetherEyeAPI.Models
{
    public class Venta
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
    }
}
