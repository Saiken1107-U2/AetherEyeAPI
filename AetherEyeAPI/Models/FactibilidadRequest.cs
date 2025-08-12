namespace AetherEyeAPI.Models
{
    // Request para validar factibilidad de producción
    public class FactibilidadRequest
    {
        public int ProductoId { get; set; }
        public int CantidadSolicitada { get; set; } = 1;
        public DateTime? FechaRequerida { get; set; }
        public bool VerificarStockCompleto { get; set; } = true;
        public bool IncluirComponentesCriticos { get; set; } = true;
    }

    // Respuesta de factibilidad
    public class FactibilidadResponse
    {
        public int ProductoId { get; set; }
        public int CantidadSolicitada { get; set; }
        public bool EsFactible { get; set; }
        public bool FactibilidadParcial { get; set; }
        public List<string> Problemas { get; set; } = new List<string>();
        public List<string> Advertencias { get; set; } = new List<string>();
        public decimal CostoTotalProduccion { get; set; }
        public decimal TiempoEstimadoMinutos { get; set; }
        public DateTime FechaEntregaEstimada { get; set; }
        public List<ComponenteProblema> ComponentesProblematicos { get; set; } = new List<ComponenteProblema>();
    }

    public class ComponenteProblema
    {
        public int ComponenteId { get; set; }
        public string ComponenteNombre { get; set; } = string.Empty;
        public string TipoProblema { get; set; } = string.Empty; // "StockInsuficiente", "ComponenteCritico", "AltaMerma"
        public decimal CantidadRequerida { get; set; }
        public int StockActual { get; set; }
        public decimal? CantidadFaltante { get; set; }
        public bool EsCritico { get; set; }
    }
}
