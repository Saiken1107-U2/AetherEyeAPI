using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AetherEyeAPI.Data;
using AetherEyeAPI.Models;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("todas")]
        public async Task<IActionResult> ObtenerTodasLasVentas()
        {
            Console.WriteLine("🔧 VentasController - Obteniendo todas las ventas para admin...");
            
            var ventas = await (from venta in _context.Ventas
                              join usuario in _context.Usuarios on venta.UsuarioId equals usuario.Id
                              join detalle in _context.DetalleVentas on venta.Id equals detalle.VentaId
                              join producto in _context.Productos on detalle.ProductoId equals producto.Id
                              select new
                              {
                                  id = venta.Id,
                                  producto = producto.Nombre,
                                  cliente = usuario.NombreCompleto,
                                  cantidad = detalle.Cantidad,
                                  total = venta.Total,
                                  estado = "Completada", // Las ventas siempre están completadas
                                  fecha = venta.Fecha // Mantenemos como DateTime para Entity Framework
                              })
                              .OrderByDescending(v => v.fecha)
                              .ToListAsync();

            // Convertir la fecha a string después de obtener los datos
            var resultado = ventas.Select(v => new
            {
                v.id,
                v.producto,
                v.cliente,
                v.cantidad,
                v.total,
                v.estado,
                fecha = v.fecha.ToString("yyyy-MM-dd")
            }).ToList();

            Console.WriteLine($"🔧 VentasController - Ventas encontradas: {resultado.Count}");
            if (resultado.Any())
            {
                var primeraVenta = resultado.First();
                Console.WriteLine($"🔧 Primera venta: Producto={primeraVenta.producto}, Cliente={primeraVenta.cliente}, Cantidad={primeraVenta.cantidad}");
            }

            return Ok(resultado);
        }

        [HttpGet("detalladas")]
        public async Task<IActionResult> ObtenerVentasDetalladas()
        {
            Console.WriteLine("🔧 VentasController - Obteniendo ventas con detalles completos...");
            
            var ventasConDetalle = await (from venta in _context.Ventas
                                        join usuario in _context.Usuarios on venta.UsuarioId equals usuario.Id
                                        select new
                                        {
                                            venta.Id,
                                            Cliente = usuario.NombreCompleto,
                                            CorreoCliente = usuario.Correo,
                                            venta.Total,
                                            Fecha = venta.Fecha,
                                            Productos = (from detalle in _context.DetalleVentas
                                                       join producto in _context.Productos on detalle.ProductoId equals producto.Id
                                                       where detalle.VentaId == venta.Id
                                                       select new
                                                       {
                                                           ProductoId = detalle.ProductoId,
                                                           Nombre = producto.Nombre,
                                                           Cantidad = detalle.Cantidad,
                                                           PrecioUnitario = detalle.PrecioUnitario,
                                                           Subtotal = detalle.Cantidad * detalle.PrecioUnitario
                                                       }).ToList()
                                        })
                                        .OrderByDescending(v => v.Fecha)
                                        .ToListAsync();

            // Agregar campo calculado CantidadProductos
            var resultado = ventasConDetalle.Select(v => new
            {
                v.Id,
                v.Cliente,
                v.CorreoCliente,
                v.Total,
                v.Fecha,
                CantidadProductos = v.Productos.Sum(p => p.Cantidad),
                v.Productos
            });

            return Ok(resultado);
        }

        [HttpGet("cliente/{usuarioId}")]
        [Authorize] // Requiere estar autenticado
        public async Task<IActionResult> ObtenerComprasDelCliente(int usuarioId)
        {
            try
            {
                var compras = await _context.Ventas
                    .Where(v => v.UsuarioId == usuarioId)
                    .Select(v => new
                    {
                        v.Id,
                        v.Fecha,
                        v.Total,
                        Estado = "Completada" // Las ventas siempre están completadas
                    })
                    .OrderByDescending(v => v.Fecha)
                    .ToListAsync();

                return Ok(compras);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        // Endpoint temporal para debug - eliminar en producción
        [HttpGet("test/{usuarioId}")]
        public async Task<IActionResult> TestComprasDelCliente(int usuarioId)
        {
            try
            {
                var todasLasVentas = await _context.Ventas.ToListAsync();
                var ventasDelUsuario = await _context.Ventas
                    .Where(v => v.UsuarioId == usuarioId)
                    .ToListAsync();

                return Ok(new
                {
                    TotalVentas = todasLasVentas.Count,
                    VentasDelUsuario = ventasDelUsuario.Count,
                    UsuarioId = usuarioId,
                    Ventas = ventasDelUsuario.Select(v => new
                    {
                        v.Id,
                        v.UsuarioId,
                        v.Fecha,
                        v.Total
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error en test", detail = ex.Message });
            }
        }

        [HttpGet("cliente/{usuarioId}/detalle/{ventaId}")]
        [Authorize] // Requiere estar autenticado
        public async Task<IActionResult> ObtenerDetalleCompra(int usuarioId, int ventaId)
        {
            var venta = await _context.Ventas
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(v => v.Id == ventaId && v.UsuarioId == usuarioId);

            if (venta == null)
                return NotFound("Compra no encontrada");

            var detalles = await _context.DetalleVentas
                .Include(dv => dv.Producto)
                .Where(dv => dv.VentaId == ventaId)
                .Select(dv => new
                {
                    dv.Id,
                    Producto = dv.Producto!.Nombre,
                    dv.Cantidad,
                    dv.PrecioUnitario,
                    Subtotal = dv.Subtotal
                })
                .ToListAsync();

            return Ok(new
            {
                Compra = new
                {
                    venta.Id,
                    venta.Fecha,
                    venta.Total,
                    Cliente = venta.Usuario!.NombreCompleto
                },
                Detalles = detalles
            });
        }

        // Endpoint temporal para generar datos de prueba
        [HttpPost("generar-datos-prueba")]
        public async Task<IActionResult> GenerarDatosPrueba()
        {
            try
            {
                // Verificar si ya existen datos
                var ventasExistentes = await _context.Ventas.CountAsync();
                if (ventasExistentes > 0)
                {
                    return Ok(new { message = "Ya existen datos de ventas" });
                }

                // Buscar usuarios existentes
                var usuarios = await _context.Usuarios.Take(3).ToListAsync();
                if (!usuarios.Any())
                {
                    return BadRequest("No hay usuarios en la base de datos");
                }

                // Buscar productos existentes
                var productos = await _context.Productos.Take(5).ToListAsync();
                if (!productos.Any())
                {
                    return BadRequest("No hay productos en la base de datos");
                }

                // Crear ventas de prueba
                var ventasPrueba = new List<Venta>();
                var random = new Random();

                for (int i = 0; i < 10; i++)
                {
                    var venta = new Venta
                    {
                        UsuarioId = usuarios[random.Next(usuarios.Count)].Id,
                        Fecha = DateTime.Now.AddDays(-random.Next(30)),
                        Total = random.Next(10000, 100000)
                    };
                    
                    _context.Ventas.Add(venta);
                    await _context.SaveChangesAsync();

                    // Crear detalle de venta
                    var producto = productos[random.Next(productos.Count)];
                    var cantidad = random.Next(1, 5);
                    
                    var detalle = new DetalleVenta
                    {
                        VentaId = venta.Id,
                        ProductoId = producto.Id,
                        Cantidad = cantidad,
                        PrecioUnitario = venta.Total / cantidad
                    };
                    
                    _context.DetalleVentas.Add(detalle);
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Datos de prueba generados exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al generar datos: {ex.Message}");
            }
        }
    }
}
