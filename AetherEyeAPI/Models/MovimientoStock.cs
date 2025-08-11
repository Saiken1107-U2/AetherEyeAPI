namespace AetherEyeAPI.Models
{
    public class MovimientoStock
    {
        public int Id { get; set; }
        public int InsumoId { get; set; }
        public required string TipoMovimiento { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public required string Motivo { get; set; }
        public string? Documento { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal StockAnterior { get; set; }
        public decimal StockNuevo { get; set; }

        // Navegación
        public virtual Insumo? Insumo { get; set; }
    }
}
