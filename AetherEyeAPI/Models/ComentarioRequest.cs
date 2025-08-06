namespace AetherEyeAPI.Models
{
    public class ComentarioRequest
    {
        public int UsuarioId { get; set; }
        public int ProductoId { get; set; }
        public required string ComentarioTexto { get; set; }
        public int Calificacion { get; set; } // 1 a 5
    }
}