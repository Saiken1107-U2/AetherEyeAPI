namespace AetherEyeAPI.Models
{
    public class Insumo
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? CodigoInterno { get; set; }
        public string? Categoria { get; set; }
        public string? UnidadMedida { get; set; }
        public decimal? CostoUnitario { get; set; }
        public decimal? StockActual { get; set; }
        public decimal? StockMinimo { get; set; }
        public decimal? StockMaximo { get; set; }
        public int? ProveedorId { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public DateTime? FechaCreacion { get; set; } 
        public DateTime? FechaActualizacion { get; set; }

        // Navegación
        public virtual Proveedor? Proveedor { get; set; }
    }
}
