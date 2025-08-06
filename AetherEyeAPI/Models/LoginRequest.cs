namespace AetherEyeAPI.Models
{
    public class LoginRequest
    {
        public required string Correo { get; set; }
        public required string Contrasena { get; set; }
    }
}