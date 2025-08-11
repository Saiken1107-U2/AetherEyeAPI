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

            var desglose = new List<CotizacionDetalleResponse>();
            decimal costoTotalProduccion = 0;

            foreach (var item in receta)
            {
                if (item.Insumo == null || (item.Insumo.StockActual ?? 0) <= 0)
                    return BadRequest($"El insumo '{item.Insumo?.Nombre}' no tiene stock suficiente para calcular el costo promedio");

                decimal costoPromedio = item.Insumo.CostoUnitario ?? 0;

                var detalle = new CotizacionDetalleResponse
                {
                    Insumo = item.Insumo.Nombre,
                    Cantidad = item.CantidadNecesaria,
                    CostoUnitario = costoPromedio
                };

                costoTotalProduccion += detalle.Subtotal;
                desglose.Add(detalle);
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
                cotizacion.Estado,
                Detalle = desglose
            });
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetCotizacionesPorUsuario(int usuarioId)
        {
            var cotizaciones = await _context.Cotizaciones
                .Include(c => c.Producto)
                .Include(c => c.Usuario)
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.Fecha)
                .Select(c => new
                {
                    c.Id,
                    c.Cantidad,
                    c.PrecioUnitario,
                    c.Total,
                    c.Fecha,
                    c.Estado,
                    Producto = new
                    {
                        c.Producto!.Id,
                        c.Producto.Nombre,
                        c.Producto.Descripcion
                    }
                })
                .ToListAsync();

            return Ok(cotizaciones);
        }

        [HttpGet("pendientes")]
        public async Task<ActionResult<IEnumerable<object>>> GetCotizacionesPendientes()
        {
            var cotizaciones = await _context.Cotizaciones
                .Include(c => c.Producto)
                .Include(c => c.Usuario)
                .Where(c => c.Estado == "Pendiente")
                .OrderByDescending(c => c.Fecha)
                .Select(c => new
                {
                    c.Id,
                    c.Cantidad,
                    c.PrecioUnitario,
                    c.Total,
                    c.Fecha,
                    c.Estado,
                    Producto = new
                    {
                        c.Producto!.Id,
                        c.Producto.Nombre,
                        c.Producto.Descripcion
                    },
                    Usuario = new
                    {
                        c.Usuario!.Id,
                        c.Usuario.NombreCompleto,
                        c.Usuario.Correo
                    }
                })
                .ToListAsync();

            return Ok(cotizaciones);
        }

        [HttpGet("todas")]
        public async Task<ActionResult<IEnumerable<object>>> GetTodasLasCotizaciones()
        {
            var cotizaciones = await _context.Cotizaciones
                .Include(c => c.Producto)
                .Include(c => c.Usuario)
                .OrderByDescending(c => c.Fecha)
                .Select(c => new
                {
                    c.Id,
                    c.Cantidad,
                    c.PrecioUnitario,
                    c.Total,
                    c.Fecha,
                    c.Estado,
                    Producto = new
                    {
                        c.Producto!.Id,
                        c.Producto.Nombre,
                        c.Producto.Descripcion
                    },
                    Usuario = new
                    {
                        c.Usuario!.Id,
                        c.Usuario.NombreCompleto,
                        c.Usuario.Correo
                    }
                })
                .ToListAsync();

            return Ok(cotizaciones);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoCotizacion(int id, [FromBody] CambiarEstadoCotizacionRequest request)
        {
            var cotizacion = await _context.Cotizaciones
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);
                
            if (cotizacion == null)
                return NotFound("Cotización no encontrada");

            var estadoAnterior = cotizacion.Estado;
            cotizacion.Estado = request.Estado;

            // Si la cotización se aprueba, crear una venta automáticamente
            if (request.Estado.Equals("Aprobada", StringComparison.OrdinalIgnoreCase) && 
                !estadoAnterior.Equals("Aprobada", StringComparison.OrdinalIgnoreCase))
            {
                await CrearVentaDesdeCotizacion(cotizacion);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Estado de cotización actualizado a: {request.Estado}" });
        }

        private async Task CrearVentaDesdeCotizacion(Cotizacion cotizacion)
        {
            // Crear la venta principal
            var venta = new Venta
            {
                UsuarioId = cotizacion.UsuarioId,
                Fecha = DateTime.Now,
                Total = cotizacion.Total
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            // Crear el detalle de la venta
            var detalleVenta = new DetalleVenta
            {
                VentaId = venta.Id,
                ProductoId = cotizacion.ProductoId,
                Cantidad = cotizacion.Cantidad,
                PrecioUnitario = cotizacion.PrecioUnitario
            };

            _context.DetalleVentas.Add(detalleVenta);
            await _context.SaveChangesAsync();
        }

        [HttpGet("detalle/{id}")]
        public async Task<IActionResult> GetDetalleCotizacion(int id)
        {
            var cotizacion = await _context.Cotizaciones
                .Include(c => c.Producto)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cotizacion == null)
                return NotFound("Cotización no encontrada");

            // Obtener el desglose de la receta
            var receta = await _context.Recetas
                .Include(r => r.Insumo)
                .Where(r => r.ProductoId == cotizacion.ProductoId)
                .ToListAsync();

            var desglose = new List<CotizacionDetalleResponse>();
            foreach (var item in receta)
            {
                var detalle = new CotizacionDetalleResponse
                {
                    Insumo = item.Insumo?.Nombre ?? "N/A",
                    Cantidad = item.CantidadNecesaria,
                    CostoUnitario = item.Insumo?.CostoUnitario ?? 0
                };
                desglose.Add(detalle);
            }

            return Ok(new
            {
                cotizacion.Id,
                cotizacion.Cantidad,
                cotizacion.PrecioUnitario,
                cotizacion.Total,
                cotizacion.Fecha,
                cotizacion.Estado,
                Producto = new
                {
                    cotizacion.Producto!.Id,
                    cotizacion.Producto.Nombre,
                    cotizacion.Producto.Descripcion
                },
                Usuario = new
                {
                    cotizacion.Usuario!.Id,
                    cotizacion.Usuario.NombreCompleto,
                    cotizacion.Usuario.Correo
                },
                Desglose = desglose
            });
        }

    }
}
