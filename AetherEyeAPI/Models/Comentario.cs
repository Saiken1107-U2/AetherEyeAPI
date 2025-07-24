namespace AetherEyeAPI.Models
{
    public class Comentario
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public string ComentarioTexto { get; set; }
        public int Calificacion { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
