namespace AetherEyeAPI.Models
{
    public class CompraInsumoRequest
    {
        public int InsumoId { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
    }

    public class CompraRequest
    {
        public int ProveedorId { get; set; }
        public required List<CompraInsumoRequest> Insumos { get; set; }
    }
}