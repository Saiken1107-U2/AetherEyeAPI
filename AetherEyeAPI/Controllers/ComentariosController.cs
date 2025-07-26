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

        [HttpPost("crear")]
        public async Task<IActionResult> CrearComentario([FromBody] ComentarioRequest request)
        {
            if (request.Calificacion < 1 || request.Calificacion > 5)
                return BadRequest("La calificación debe estar entre 1 y 5.");

            // (Opcional) Validar que el usuario haya comprado el producto
            bool haComprado = await _context.Ventas
                .AnyAsync(v => v.UsuarioId == request.UsuarioId &&
                               _context.DetalleVentas.Any(d => d.VentaId == v.Id && d.ProductoId == request.ProductoId));

            if (!haComprado)
                return BadRequest("El usuario no ha comprado este producto.");

            var comentario = new Comentario
            {
                UsuarioId = request.UsuarioId,
                ProductoId = request.ProductoId,
                ComentarioTexto = request.ComentarioTexto,
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
                comentario.ComentarioTexto
            });
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
                    Usuario = c.Usuario.NombreCompleto,
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
                    Usuario = c.Usuario.NombreCompleto,
                    Producto = c.Producto.Nombre,
                    c.Calificacion,
                    c.ComentarioTexto,
                    c.Fecha
                })
                .ToListAsync();

            return Ok(comentarios);
        }

    }
}
