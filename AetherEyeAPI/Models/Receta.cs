namespace AetherEyeAPI.Models
{
    public class Receta
    {
        public int Id { get; set; }

        // Producto que se está fabricando
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        // Insumo necesario (componente, sensor, etc.)
        public int InsumoId { get; set; }
        public Insumo? Insumo { get; set; }

        // Cantidad necesaria por unidad del producto
        public int CantidadNecesaria { get; set; }

        // Unidad de medida específica para esta receta
        public string UnidadMedida { get; set; } = string.Empty;

        // Fecha de creación
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
