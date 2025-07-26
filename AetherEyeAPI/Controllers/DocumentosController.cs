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
    public class DocumentosController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public DocumentosController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/Documentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Documento>>> GetDocumentos()
        {
            return await _context.Documentos.ToListAsync();
        }

        // GET: api/Documentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Documento>> GetDocumento(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);

            if (documento == null)
            {
                return NotFound();
            }

            return documento;
        }

        // PUT: api/Documentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumento(int id, Documento documento)
        {
            if (id != documento.Id)
            {
                return BadRequest();
            }

            _context.Entry(documento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentoExists(id))
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

        // POST: api/Documentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Documento>> PostDocumento(Documento documento)
        {
            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocumento", new { id = documento.Id }, documento);
        }

        // DELETE: api/Documentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumento(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }

            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentoExists(int id)
        {
            return _context.Documentos.Any(e => e.Id == id);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearDocumento([FromBody] DocumentoRequest request)
        {
            var producto = await _context.Productos.FindAsync(request.ProductoId);
            if (producto == null)
                return NotFound("Producto no encontrado");

            var documento = new Documento
            {
                ProductoId = request.ProductoId,
                Titulo = request.Titulo,
                Descripcion = request.Descripcion,
                UrlArchivo = request.UrlArchivo,
                Fecha = DateTime.Now
            };

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                documento.Id,
                documento.ProductoId,
                documento.Titulo,
                documento.UrlArchivo
            });
        }

        [HttpGet("producto/{productoId}")]
        public async Task<IActionResult> ObtenerPorProducto(int productoId)
        {
            var documentos = await _context.Documentos
                .Where(d => d.ProductoId == productoId)
                .Select(d => new
                {
                    d.Id,
                    d.Titulo,
                    d.Descripcion,
                    d.UrlArchivo,
                    d.Fecha
                })
                .ToListAsync();

            return Ok(documentos);
        }

        [HttpGet("cliente/{usuarioId}")]
        public async Task<IActionResult> DocumentosCliente(int usuarioId)
        {
            var productosComprados = await _context.Ventas
                .Where(v => v.UsuarioId == usuarioId)
                .SelectMany(v => _context.DetalleVentas
                    .Where(dv => dv.VentaId == v.Id)
                    .Select(dv => dv.ProductoId))
                .Distinct()
                .ToListAsync();

            var documentos = await _context.Documentos
                .Where(d => productosComprados.Contains(d.ProductoId))
                .Select(d => new
                {
                    d.Id,
                    d.ProductoId,
                    d.Titulo,
                    d.Descripcion,
                    d.UrlArchivo,
                    d.Fecha
                })
                .ToListAsync();

            return Ok(documentos);
        }

    }
}
