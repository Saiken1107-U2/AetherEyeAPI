namespace AetherEyeAPI.Models
{
    public class ChartData
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string? Color { get; set; }
    }

    public class StockByCategory
    {
        public string Categoria { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal ValorTotal { get; set; }
    }

    public class MovementTrend
    {
        public DateTime Fecha { get; set; }
        public int Entradas { get; set; }
        public int Salidas { get; set; }
        public int Ajustes { get; set; }
    }
}
