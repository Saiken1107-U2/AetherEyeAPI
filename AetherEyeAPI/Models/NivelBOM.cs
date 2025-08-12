namespace AetherEyeAPI.Models
{
    // Modelo para representar niveles en la estructura BOM
    public class NivelBOM
    {
        public int Nivel { get; set; }
        public int ProductoId { get; set; }
        public int ComponenteId { get; set; }
        public string ComponenteNombre { get; set; } = string.Empty;
        public string TipoComponente { get; set; } = string.Empty; // "Insumo" o "Subproducto"
        public decimal CantidadNecesaria { get; set; }
        public int OrdenSecuencia { get; set; }
        public bool EsCritico { get; set; }
        public decimal TiempoPreparacion { get; set; }
    }
}
