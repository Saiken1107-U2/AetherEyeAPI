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
    public class ComprasController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public ComprasController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/Compras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compra>>> GetCompras()
        {
            return await _context.Compras.ToListAsync();
        }

        // GET: api/Compras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Compra>> GetCompra(int id)
        {
            var compra = await _context.Compras.FindAsync(id);

            if (compra == null)
            {
                return NotFound();
            }

            return compra;
        }

        // PUT: api/Compras/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompra(int id, Compra compra)
        {
            if (id != compra.Id)
            {
                return BadRequest();
            }

            _context.Entry(compra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompraExists(id))
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

        // POST: api/Compras
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Compra>> PostCompra(Compra compra)
        {
            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompra", new { id = compra.Id }, compra);
        }

        // DELETE: api/Compras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompra(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }

            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.Id == id);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCompra([FromBody] CompraRequest request)
        {
            if (request.Insumos == null || !request.Insumos.Any())
                return BadRequest("Debe incluir al menos un insumo en la compra.");

            decimal totalCompra = 0;

            var compra = new Compra
            {
                ProveedorId = request.ProveedorId,
                Fecha = DateTime.Now
            };

            _context.Compras.Add(compra);
            await _context.SaveChangesAsync(); // Guardamos para obtener el ID

            foreach (var item in request.Insumos)
            {
                var insumo = await _context.Insumos.FindAsync(item.InsumoId);
                if (insumo == null)
                    return NotFound($"Insumo con ID {item.InsumoId} no encontrado.");

                // Cálculo de nuevo costo promedio
                int stockAnterior = insumo.StockActual;
                decimal costoAnterior = insumo.CostoUnitario;
                int cantidadNueva = item.Cantidad;
                decimal costoNuevo = item.CostoUnitario;

                int nuevoStock = stockAnterior + cantidadNueva;

                decimal nuevoCostoPromedio = nuevoStock == 0
                    ? costoNuevo
                    : ((stockAnterior * costoAnterior) + (cantidadNueva * costoNuevo)) / nuevoStock;

                // Actualizar insumo
                insumo.StockActual = nuevoStock;
                insumo.CostoUnitario = Math.Round(nuevoCostoPromedio, 2);
                insumo.FechaUltimaActualizacion = DateTime.Now;

                // Detalle de compra
                var detalle = new DetalleCompra
                {
                    CompraId = compra.Id,
                    InsumoId = item.InsumoId,
                    Cantidad = cantidadNueva,
                    CostoUnitario = costoNuevo
                };

                _context.DetalleCompras.Add(detalle);
                totalCompra += cantidadNueva * costoNuevo;
            }

            // Actualizar total de la compra
            compra.Total = Math.Round(totalCompra, 2);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                compra.Id,
                compra.Fecha,
                compra.ProveedorId,
                compra.Total,
                InsumosComprados = request.Insumos
            });
        }
    }
}