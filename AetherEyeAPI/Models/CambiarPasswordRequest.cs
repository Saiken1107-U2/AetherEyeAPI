namespace AetherEyeAPI.Models
{
    public class CambiarPasswordRequest
    {
        public required string ContrasenaActual { get; set; }
        public required string NuevaContrasena { get; set; }
    }
}