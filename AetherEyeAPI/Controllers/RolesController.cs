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
    public class RolesController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public RolesController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRoles()
        {
            try
            {
                // Obtener todos los roles primero y luego filtrar duplicados en memoria
                var todosLosRoles = await _context.Roles.ToListAsync();
                
                // Filtrar duplicados por nombre (ignorando mayúsculas)
                var rolesUnicos = todosLosRoles
                    .GroupBy(r => r.Nombre.ToLower())
                    .Select(g => g.First())
                    .OrderBy(r => r.Nombre)
                    .ToList();

                return Ok(rolesUnicos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener roles: {ex.Message}");
            }
        }

        // GET: api/Roles/debug - Endpoint temporal para ver todos los roles incluyendo duplicados
        [HttpGet("debug")]
        public async Task<ActionResult<IEnumerable<object>>> GetRolesDebug()
        {
            var todosLosRoles = await _context.Roles
                .Select(r => new { r.Id, r.Nombre })
                .OrderBy(r => r.Nombre)
                .ToListAsync();
                
            Console.WriteLine($"🔍 Total de roles en BD: {todosLosRoles.Count}");
            foreach(var rol in todosLosRoles)
            {
                Console.WriteLine($"🔍 Rol: ID={rol.Id}, Nombre='{rol.Nombre}'");
            }
                
            return todosLosRoles;
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);

            if (rol == null)
            {
                return NotFound();
            }

            return rol;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, Rol rol)
        {
            if (id != rol.Id)
            {
                return BadRequest();
            }

            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol(Rol rol)
        {
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRol", new { id = rol.Id }, rol);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
