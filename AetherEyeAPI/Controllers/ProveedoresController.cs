using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AetherEyeAPI.Data;
using AetherEyeAPI.Models;

namespace AetherEyeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public ProveedoresController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/Proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores()
        {
            try
            {
                var proveedores = await _context.Proveedores
                    .OrderBy(p => p.Nombre)
                    .ToListAsync();
                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener proveedores", error = ex.Message });
            }
        }

        // GET: api/Proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            try
            {
                var proveedor = await _context.Proveedores.FindAsync(id);

                if (proveedor == null)
                {
                    return NotFound(new { message = "Proveedor no encontrado" });
                }

                return Ok(proveedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener proveedor", error = ex.Message });
            }
        }

        // POST: api/Proveedores
        [HttpPost]
        public async Task<ActionResult<Proveedor>> PostProveedor(ProveedorRequest request)
        {
            try
            {
                // Verificar si ya existe un proveedor con el mismo nombre
                var proveedorExistente = await _context.Proveedores
                    .FirstOrDefaultAsync(p => p.Nombre.ToLower() == request.Nombre.ToLower());

                if (proveedorExistente != null)
                {
                    return BadRequest(new { message = "Ya existe un proveedor con este nombre" });
                }

                var proveedor = new Proveedor
                {
                    Nombre = request.Nombre,
                    NombreContacto = request.NombreContacto,
                    Correo = request.Correo,
                    Telefono = request.Telefono,
                    Direccion = request.Direccion,
                    Ciudad = request.Ciudad,
                    Pais = request.Pais,
                    CodigoPostal = request.CodigoPostal,
                    SitioWeb = request.SitioWeb,
                    Descripcion = request.Descripcion,
                    EstaActivo = true,
                    FechaRegistro = DateTime.UtcNow
                };

                _context.Proveedores.Add(proveedor);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProveedor", new { id = proveedor.Id }, proveedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear proveedor", error = ex.Message });
            }
        }

        // PUT: api/Proveedores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProveedor(int id, ProveedorRequest request)
        {
            try
            {
                var proveedor = await _context.Proveedores.FindAsync(id);
                if (proveedor == null)
                {
                    return NotFound(new { message = "Proveedor no encontrado" });
                }

                // Verificar si el nombre ya existe en otro proveedor
                var proveedorExistente = await _context.Proveedores
                    .FirstOrDefaultAsync(p => p.Nombre.ToLower() == request.Nombre.ToLower() && p.Id != id);

                if (proveedorExistente != null)
                {
                    return BadRequest(new { message = "Ya existe otro proveedor con este nombre" });
                }

                // Actualizar los campos
                proveedor.Nombre = request.Nombre;
                proveedor.NombreContacto = request.NombreContacto;
                proveedor.Correo = request.Correo;
                proveedor.Telefono = request.Telefono;
                proveedor.Direccion = request.Direccion;
                proveedor.Ciudad = request.Ciudad;
                proveedor.Pais = request.Pais;
                proveedor.CodigoPostal = request.CodigoPostal;
                proveedor.SitioWeb = request.SitioWeb;
                proveedor.Descripcion = request.Descripcion;

                _context.Entry(proveedor).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Proveedor actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar proveedor", error = ex.Message });
            }
        }



        // DELETE: api/Proveedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            try
            {
                var proveedor = await _context.Proveedores.FindAsync(id);
                if (proveedor == null)
                {
                    return NotFound(new { message = "Proveedor no encontrado" });
                }

                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Proveedor eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar proveedor", error = ex.Message });
            }
        }

        private bool ProveedorExists(int id)
        {
            return _context.Proveedores.Any(e => e.Id == id);
        }
    }
}
