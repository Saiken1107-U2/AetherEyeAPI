﻿namespace AetherEyeAPI.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
    }
}
