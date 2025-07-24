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
    public class DetalleVentasController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public DetalleVentasController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/DetalleVentas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleVenta>>> GetDetalleVentas()
        {
            return await _context.DetalleVentas.ToListAsync();
        }

        // GET: api/DetalleVentas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleVenta>> GetDetalleVenta(int id)
        {
            var detalleVenta = await _context.DetalleVentas.FindAsync(id);

            if (detalleVenta == null)
            {
                return NotFound();
            }

            return detalleVenta;
        }

        // PUT: api/DetalleVentas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleVenta(int id, DetalleVenta detalleVenta)
        {
            if (id != detalleVenta.Id)
            {
                return BadRequest();
            }

            _context.Entry(detalleVenta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleVentaExists(id))
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

        // POST: api/DetalleVentas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetalleVenta>> PostDetalleVenta(DetalleVenta detalleVenta)
        {
            _context.DetalleVentas.Add(detalleVenta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleVenta", new { id = detalleVenta.Id }, detalleVenta);
        }

        // DELETE: api/DetalleVentas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleVenta(int id)
        {
            var detalleVenta = await _context.DetalleVentas.FindAsync(id);
            if (detalleVenta == null)
            {
                return NotFound();
            }

            _context.DetalleVentas.Remove(detalleVenta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetalleVentaExists(int id)
        {
            return _context.DetalleVentas.Any(e => e.Id == id);
        }
    }
}
