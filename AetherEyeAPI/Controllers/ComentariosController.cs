using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AetherEyeAPI.Data;
using AetherEyeAPI.Models;

namespace AetherEyeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public ComentariosController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/Comentarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comentario>>> GetComentarios()
        {
            return await _context.Comentarios.ToListAsync();
        }

        // GET: api/Comentarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comentario>> GetComentario(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            return comentario;
        }

        // PUT: api/Comentarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComentario(int id, Comentario comentario)
        {
            if (id != comentario.Id)
            {
                return BadRequest();
            }

            _context.Entry(comentario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComentarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Comentarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Comentario>> PostComentario(Comentario comentario)
        {
            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComentario", new { id = comentario.Id }, comentario);
        }

        // DELETE: api/Comentarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentario(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null)
            {
                return NotFound();
            }

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComentarioExists(int id)
        {
            return _context.Comentarios.Any(e => e.Id == id);
        }

        [HttpGet("productos-comprados/{usuarioId}")]
        public async Task<IActionResult> ObtenerProductosComprados(int usuarioId)
        {
            try
            {
                // Verificar si el usuario existe
                var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId);
                if (!usuarioExiste)
                {
                    return BadRequest($"Usuario {usuarioId} no existe");
                }

                // Buscar productos comprados usando una consulta optimizada
                var productosComprados = await _context.DetalleVentas
                    .Include(d => d.Producto)
                    .Include(d => d.Venta)
                    .Where(d => d.Venta!.UsuarioId == usuarioId)
                    .Where(d => d.Producto != null)
                    .Select(d => new
                    {
                        ProductoId = d.ProductoId,
                        ProductoNombre = d.Producto!.Nombre,
                        FechaCompra = d.Venta!.Fecha,
                        VentaId = d.VentaId
                    })
                    .GroupBy(p => p.ProductoId)
                    .Select(g => g.OrderByDescending(x => x.FechaCompra).First()) // Tomar la compra más reciente
                    .OrderByDescending(p => p.FechaCompra)
                    .ToListAsync();

                return Ok(productosComprados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener productos comprados", detail = ex.Message });
            }
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearComentario([FromBody] ComentarioRequest request)
        {
            try
            {
                // Validaciones básicas
                if (request == null)
                    return BadRequest("Datos del comentario son requeridos.");

                if (string.IsNullOrWhiteSpace(request.ComentarioTexto))
                    return BadRequest("El texto del comentario es requerido.");

                if (request.Calificacion < 1 || request.Calificacion > 5)
                    return BadRequest("La calificación debe estar entre 1 y 5.");

                // Verificar que el producto y usuario existen, y que el usuario haya comprado el producto
                var validacion = await _context.DetalleVentas
                    .Include(d => d.Producto)
                    .Include(d => d.Venta)
                    .Where(d => d.Venta!.UsuarioId == request.UsuarioId && 
                               d.ProductoId == request.ProductoId)
                    .FirstOrDefaultAsync();

                if (validacion == null)
                    return BadRequest("Solo puedes comentar productos que hayas comprado. Por favor, selecciona un producto de tu historial de compras.");

                // Verificar si ya existe un comentario del usuario para este producto
                var comentarioExistente = await _context.Comentarios
                    .Where(c => c.UsuarioId == request.UsuarioId && c.ProductoId == request.ProductoId)
                    .FirstOrDefaultAsync();

                if (comentarioExistente != null)
                    return BadRequest("Ya has comentado este producto. Solo se permite un comentario por producto.");

                // Crear el comentario
                var comentario = new Comentario
                {
                    UsuarioId = request.UsuarioId,
                    ProductoId = request.ProductoId,
                    ComentarioTexto = request.ComentarioTexto.Trim(),
                    Calificacion = request.Calificacion,
                    Fecha = DateTime.Now
                };

                _context.Comentarios.Add(comentario);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    comentario.Id,
                    comentario.ProductoId,
                    comentario.UsuarioId,
                    comentario.Calificacion,
                    comentario.ComentarioTexto,
                    Mensaje = "Comentario creado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear comentario: {ex.Message}");
            }
        }

        [HttpGet("producto/{productoId}")]
        public async Task<IActionResult> ObtenerPorProducto(int productoId)
        {
            var comentarios = await _context.Comentarios
                .Where(c => c.ProductoId == productoId)
                .Include(c => c.Usuario)
                .Select(c => new
                {
                    c.Id,
                    Usuario = c.Usuario!.NombreCompleto,
                    c.Calificacion,
                    c.ComentarioTexto,
                    c.Fecha
                })
                .ToListAsync();

            return Ok(comentarios);
        }

        [HttpGet("admin")]
        public async Task<IActionResult> ObtenerTodos()
        {
            var comentarios = await _context.Comentarios
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .Select(c => new
                {
                    c.Id,
                    Usuario = c.Usuario!.NombreCompleto,
                    Producto = c.Producto!.Nombre,
                    c.Calificacion,
                    c.ComentarioTexto,
                    c.Fecha
                })
                .ToListAsync();

            return Ok(comentarios);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerPorUsuario(int usuarioId)
        {
            var comentarios = await _context.Comentarios
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .Where(c => c.UsuarioId == usuarioId)
                .Select(c => new
                {
                    c.Id,
                    Usuario = c.Usuario!.NombreCompleto,
                    Producto = c.Producto!.Nombre,
                    c.Calificacion,
                    c.ComentarioTexto,
                    c.Fecha
                })
                .ToListAsync();

            return Ok(comentarios);
        }

        [HttpGet("lista")]
        public async Task<IActionResult> ObtenerTodosLosComentarios()
        {
            Console.WriteLine("🔧 ComentariosController - Obteniendo todos los comentarios...");
            
            var comentarios = await _context.Comentarios
                .Include(c => c.Producto)
                .Include(c => c.Usuario)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();

            // Mapear los datos después de obtenerlos de la base de datos
            var resultado = comentarios.Select(c => new
            {
                id = c.Id,
                producto = c.Producto?.Nombre ?? "Producto no disponible",
                cliente = c.Usuario?.NombreCompleto ?? "Cliente no disponible",
                contenido = c.ComentarioTexto,
                fecha = c.Fecha.ToString("yyyy-MM-dd")
            }).ToList();

            Console.WriteLine($"🔧 ComentariosController - Comentarios encontrados: {resultado.Count}");
            if (resultado.Any())
            {
                var primerComentario = resultado.First();
                Console.WriteLine($"🔧 Primer comentario: Producto={primerComentario.producto}, Cliente={primerComentario.cliente}");
            }

            return Ok(resultado);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarComentario(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null) return NotFound();

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("estadisticas/{productoId}")]
        public async Task<IActionResult> ObtenerEstadisticasProducto(int productoId)
        {
            var comentarios = await _context.Comentarios
                .Where(c => c.ProductoId == productoId)
                .ToListAsync();

            if (!comentarios.Any())
            {
                return Ok(new
                {
                    TotalComentarios = 0,
                    CalificacionPromedio = 0.0,
                    DistribucionCalificaciones = new int[5]
                });
            }

            var estadisticas = new
            {
                TotalComentarios = comentarios.Count,
                CalificacionPromedio = Math.Round(comentarios.Average(c => c.Calificacion), 1),
                DistribucionCalificaciones = new int[]
                {
                    comentarios.Count(c => c.Calificacion == 1),
                    comentarios.Count(c => c.Calificacion == 2),
                    comentarios.Count(c => c.Calificacion == 3),
                    comentarios.Count(c => c.Calificacion == 4),
                    comentarios.Count(c => c.Calificacion == 5)
                }
            };

            return Ok(estadisticas);
        }
    }
}
