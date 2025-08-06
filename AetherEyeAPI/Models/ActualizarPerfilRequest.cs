namespace AetherEyeAPI.Models
{
    public class ActualizarPerfilRequest
    {
        public required string NombreCompleto { get; set; }
        public required string Correo { get; set; }
    }
}