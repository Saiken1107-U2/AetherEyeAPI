namespace AetherEyeAPI.Models
{
    // Modelo para representar la explosión de materiales
    public class ExplosionMateriales
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public int CantidadProductos { get; set; }
        public List<ExplosionItem> Items { get; set; } = new List<ExplosionItem>();
        public decimal CostoTotalMateriales { get; set; }
        public decimal CostoTotalConMerma { get; set; }
        public bool TieneFaltantes { get; set; }
        public List<string> AlertasFaltantes { get; set; } = new List<string>();
    }

    public class ExplosionItem
    {
        public int? InsumoId { get; set; }
        public int? SubproductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // "Insumo" o "Subproducto"
        public decimal CantidadUnitaria { get; set; }
        public decimal CantidadTotal { get; set; }
        public decimal CantidadConMerma { get; set; }
        public string UnidadMedida { get; set; } = string.Empty;
        public decimal CostoUnitario { get; set; }
        public decimal CostoTotal { get; set; }
        public decimal PorcentajeMerma { get; set; }
        public int Nivel { get; set; }
        public int StockActual { get; set; }
        public int StockDisponible { get; set; }
        public bool TieneFaltante { get; set; }
        public bool EsCritico { get; set; }
        public decimal? TiempoPreparacion { get; set; }
        public string? Observaciones { get; set; }
        public List<ExplosionItem> SubItems { get; set; } = new List<ExplosionItem>();
    }

    // Request para calcular explosión
    public class ExplosionRequest
    {
        public int ProductoId { get; set; }
        public int CantidadProductos { get; set; } = 1;
        public bool IncluirSubrecetas { get; set; } = true;
        public bool VerificarStock { get; set; } = true;
    }

    // Modelo para solicitud de compra generada automáticamente
    public class SolicitudCompraAutomatica
    {
        public int ProductoObjetivo { get; set; }
        public int CantidadObjetivo { get; set; }
        public List<ItemSolicitudCompra> Items { get; set; } = new List<ItemSolicitudCompra>();
        public decimal MontoEstimado { get; set; }
        public DateTime FechaNecesaria { get; set; }
    }

    public class ItemSolicitudCompra
    {
        public int InsumoId { get; set; }
        public string InsumoNombre { get; set; } = string.Empty;
        public decimal CantidadFaltante { get; set; }
        public string UnidadMedida { get; set; } = string.Empty;
        public decimal CostoUnitarioEstimado { get; set; }
        public decimal MontoTotal { get; set; }
        public int? ProveedorPreferido { get; set; }
        public bool EsCritico { get; set; }
    }
}
