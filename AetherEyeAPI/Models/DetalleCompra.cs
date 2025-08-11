namespace AetherEyeAPI.Models
{
    public class DetalleCompra
    {
        public int Id { get; set; }

        public int CompraId { get; set; }
        public Compra? Compra { get; set; }

        public int InsumoId { get; set; }
        public Insumo? Insumo { get; set; }

        public decimal Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }

        public decimal Subtotal => Cantidad * CostoUnitario;
    }
}
