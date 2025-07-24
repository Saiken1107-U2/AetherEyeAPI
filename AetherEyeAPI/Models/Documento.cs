namespace AetherEyeAPI.Models
{
    public class Documento
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? UrlArchivo { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
