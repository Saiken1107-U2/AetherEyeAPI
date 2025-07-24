namespace AetherEyeAPI.Models
{
    public class Insumo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string UnidadMedida { get; set; }
        public decimal CostoUnitario { get; set; }
        public int StockActual { get; set; }
        public DateTime FechaUltimaActualizacion { get; set; } = DateTime.Now;
    }
}
