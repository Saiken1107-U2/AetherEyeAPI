namespace AetherEyeAPI.Models
{
    // Request para optimización de secuencias
    public class OptimizacionRequest
    {
        public int ProductoId { get; set; }
        public string CriterioOptimizacion { get; set; } = "TiempoCritico"; // TiempoCritico, Costo, Stock, Merma
        public bool AplicarCambios { get; set; } = false;
    }

    // Respuesta de optimización
    public class OptimizacionResponse
    {
        public int ProductoId { get; set; }
        public string CriterioAplicado { get; set; } = string.Empty;
        public List<ComponenteOptimizado> SecuenciaOptimizada { get; set; } = new List<ComponenteOptimizado>();
        public bool CambiosAplicados { get; set; }
        public decimal TiempoAhorrado { get; set; }
        public decimal CostoOptimizado { get; set; }
    }

    public class ComponenteOptimizado
    {
        public int Id { get; set; }
        public string ComponenteNombre { get; set; } = string.Empty;
        public int OrdenAnterior { get; set; }
        public int OrdenNuevo { get; set; }
        public bool EsCritico { get; set; }
        public decimal? TiempoPreparacion { get; set; }
        public decimal? CostoUnitario { get; set; }
        public int? StockActual { get; set; }
    }
}
