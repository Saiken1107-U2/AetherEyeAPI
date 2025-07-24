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
    public class InsumosController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public InsumosController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/Insumos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Insumo>>> GetInsumos()
        {
            return await _context.Insumos.ToListAsync();
        }

        // GET: api/Insumos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Insumo>> GetInsumo(int id)
        {
            var insumo = await _context.Insumos.FindAsync(id);

            if (insumo == null)
            {
                return NotFound();
            }

            return insumo;
        }

        // PUT: api/Insumos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsumo(int id, Insumo insumo)
        {
            if (id != insumo.Id)
            {
                return BadRequest();
            }

            _context.Entry(insumo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsumoExists(id))
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

        // POST: api/Insumos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Insumo>> PostInsumo(Insumo insumo)
        {
            _context.Insumos.Add(insumo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInsumo", new { id = insumo.Id }, insumo);
        }

        // DELETE: api/Insumos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsumo(int id)
        {
            var insumo = await _context.Insumos.FindAsync(id);
            if (insumo == null)
            {
                return NotFound();
            }

            _context.Insumos.Remove(insumo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InsumoExists(int id)
        {
            return _context.Insumos.Any(e => e.Id == id);
        }
    }
}
