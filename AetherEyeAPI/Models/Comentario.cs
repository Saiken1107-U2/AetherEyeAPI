namespace AetherEyeAPI.Models
{
    public class Comentario
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public required string ComentarioTexto { get; set; }
        public int Calificacion { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        
        // Campos para seguimiento de comentarios
        public string EstadoSeguimiento { get; set; } = "Pendiente"; // Pendiente, En revision, Respondido, Resuelto, Archivado
        public string? RespuestaAdmin { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public int? AdminId { get; set; } // ID del administrador que respondió
        public Usuario? Admin { get; set; }
        public string? NotasInternas { get; set; } // Notas privadas para el equipo admin
        public int Prioridad { get; set; } = 2; // 1=Alta, 2=Media, 3=Baja
        public bool RequiereAccion { get; set; } = false; // Si requiere acción específica
        public string? CategoriaProblema { get; set; } // Queja, Sugerencia, Consulta, Problema técnico, etc.
    }
}
