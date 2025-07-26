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
    public class VentasController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public VentasController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/Ventas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            return await _context.Ventas.ToListAsync();
        }

        // GET: api/Ventas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);

            if (venta == null)
            {
                return NotFound();
            }

            return venta;
        }

        // PUT: api/Ventas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenta(int id, Venta venta)
        {
            if (id != venta.Id)
            {
                return BadRequest();
            }

            _context.Entry(venta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VentaExists(id))
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

        // POST: api/Ventas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Venta>> PostVenta(Venta venta)
        {
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenta", new { id = venta.Id }, venta);
        }

        // DELETE: api/Ventas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenta(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaRequest request)
        {
            if (request.Productos == null || !request.Productos.Any())
                return BadRequest("Debe incluir al menos un producto en la venta.");

            var venta = new Venta
            {
                UsuarioId = request.UsuarioId,
                Fecha = DateTime.Now
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync(); // para obtener venta.Id

            decimal totalVenta = 0;

            foreach (var item in request.Productos)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto == null)
                    return NotFound($"Producto con ID {item.ProductoId} no encontrado.");

                var detalle = new DetalleVenta
                {
                    VentaId = venta.Id,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario
                };

                _context.DetalleVentas.Add(detalle);
                totalVenta += item.Cantidad * item.PrecioUnitario;
            }

            venta.Total = Math.Round(totalVenta, 2);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                venta.Id,
                venta.UsuarioId,
                venta.Fecha,
                venta.Total
            });
        }

    }
}
