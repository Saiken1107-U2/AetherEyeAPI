namespace AetherEyeAPI.Models
{
    public class CotizacionDetalleResponse
    {
        public string Insumo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Subtotal => Math.Round(Cantidad * CostoUnitario, 2);
    }
}