namespace AetherEyeAPI.Models
{
    public class AlertItem
    {
        public int InsumoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal StockActual { get; set; }
        public decimal StockMinimo { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Urgencia { get; set; } = string.Empty; // "Crítico", "Bajo", "Medio"
    }

    public class TopItem
    {
        public int InsumoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int TotalMovimientos { get; set; }
        public decimal CantidadTotal { get; set; }
    }
}
