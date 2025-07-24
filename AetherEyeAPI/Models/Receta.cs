namespace AetherEyeAPI.Models
{
    public class Receta
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int InsumoId { get; set; }
        public Insumo? Insumo { get; set; }

        public decimal CantidadNecesaria { get; set; }
    }
}
