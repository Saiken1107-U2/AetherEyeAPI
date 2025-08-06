namespace AetherEyeAPI.Models
{
    public class Faq
    {
        public int Id { get; set; }
        public required string Pregunta { get; set; }
        public required string Respuesta { get; set; }
    }
}
