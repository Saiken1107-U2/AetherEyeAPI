namespace AetherEyeAPI.Models
{
    public class CompraInsumoRequest
    {
        public int InsumoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
    }

    public class CompraRequest
    {
        public int ProveedorId { get; set; }
        public string? NumeroFactura { get; set; }
        public string? Observaciones { get; set; }
        public required List<CompraInsumoRequest> Insumos { get; set; }
    }
}