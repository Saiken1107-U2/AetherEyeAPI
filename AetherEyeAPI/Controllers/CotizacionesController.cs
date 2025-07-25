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
    public class CotizacionesController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public CotizacionesController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/Cotizaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cotizacion>>> GetCotizaciones()
        {
            return await _context.Cotizaciones.ToListAsync();
        }

        // GET: api/Cotizaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cotizacion>> GetCotizacion(int id)
        {
            var cotizacion = await _context.Cotizaciones.FindAsync(id);

            if (cotizacion == null)
            {
                return NotFound();
            }

            return cotizacion;
        }

        // PUT: api/Cotizaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCotizacion(int id, Cotizacion cotizacion)
        {
            if (id != cotizacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(cotizacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CotizacionExists(id))
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

        // POST: api/Cotizaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cotizacion>> PostCotizacion(Cotizacion cotizacion)
        {
            _context.Cotizaciones.Add(cotizacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCotizacion", new { id = cotizacion.Id }, cotizacion);
        }

        // DELETE: api/Cotizaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCotizacion(int id)
        {
            var cotizacion = await _context.Cotizaciones.FindAsync(id);
            if (cotizacion == null)
            {
                return NotFound();
            }

            _context.Cotizaciones.Remove(cotizacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CotizacionExists(int id)
        {
            return _context.Cotizaciones.Any(e => e.Id == id);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearCotizacion([FromBody] CotizacionRequest request)
        {
            var producto = await _context.Productos.FindAsync(request.ProductoId);
            if (producto == null)
                return NotFound("Producto no encontrado");

            var receta = await _context.Recetas
                .Include(r => r.Insumo)
                .Where(r => r.ProductoId == request.ProductoId)
                .ToListAsync();

            if (!receta.Any())
                return BadRequest("El producto no tiene receta definida");

            decimal costoTotalProduccion = 0;

            foreach (var item in receta)
            {
                if (item.Insumo == null || item.Insumo.StockActual <= 0)
                    return BadRequest($"El insumo '{item.Insumo?.Nombre}' no tiene stock suficiente para calcular el costo promedio");

                // Método de costeo por promedio: costo promedio = costo unitario actual (ya almacenado) 
                // NOTA: asumes que el costoUnitario ya fue calculado tras las compras
                decimal costoPromedio = item.Insumo.CostoUnitario;
                decimal costoInsumo = costoPromedio * item.CantidadNecesaria;

                costoTotalProduccion += costoInsumo;
            }

            // (Opcional) Agregar margen de utilidad
            decimal margenGanancia = 0.20m; // 20%
            decimal precioFinalUnitario = costoTotalProduccion * (1 + margenGanancia);

            var cotizacion = new Cotizacion
            {
                UsuarioId = request.UsuarioId,
                ProductoId = request.ProductoId,
                Cantidad = request.Cantidad,
                PrecioUnitario = Math.Round(precioFinalUnitario, 2),
                Total = Math.Round(precioFinalUnitario * request.Cantidad, 2),
                Fecha = DateTime.Now,
                Estado = "Pendiente"
            };

            _context.Cotizaciones.Add(cotizacion);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                cotizacion.Id,
                cotizacion.ProductoId,
                cotizacion.Cantidad,
                cotizacion.PrecioUnitario,
                cotizacion.Total,
                cotizacion.Estado
            });
        }

    }
}
