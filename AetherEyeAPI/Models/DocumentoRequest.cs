namespace AetherEyeAPI.Models
{
    public class DocumentoRequest
    {
        public int ProductoId { get; set; }
        public required string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public required string UrlArchivo { get; set; } // Puede ser PDF, imagen, video, etc.
    }
}