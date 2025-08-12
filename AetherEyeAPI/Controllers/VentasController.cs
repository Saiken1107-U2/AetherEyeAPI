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
        public async Task<ActionResult<IEnumerable<VentaResponse>>> GetVentas()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(dv => dv.Producto)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            var ventasResponse = ventas.Select(v => new VentaResponse
            {
                Id = v.Id,
                NombreCliente = v.NombreCliente ?? v.Usuario?.NombreCompleto,
                CorreoCliente = v.CorreoCliente ?? v.Usuario?.Correo,
                TelefonoCliente = v.TelefonoCliente,
                DireccionCliente = v.DireccionCliente,
                Fecha = v.Fecha,
                Estado = v.Estado,
                MetodoPago = v.MetodoPago,
                Observaciones = v.Observaciones,
                NumeroFactura = v.NumeroFactura,
                Subtotal = v.Subtotal,
                Impuestos = v.Impuestos,
                Descuento = v.Descuento,
                Total = v.Total,
                VendedorNombre = v.Usuario?.NombreCompleto,
                Productos = v.DetallesVenta.Select(dv => new DetalleVentaResponse
                {
                    Id = dv.Id,
                    ProductoId = dv.ProductoId,
                    ProductoNombre = dv.Producto?.Nombre,
                    Cantidad = dv.Cantidad,
                    PrecioUnitario = dv.PrecioUnitario,
                    Subtotal = dv.Subtotal
                }).ToList()
            }).ToList();

            return Ok(ventasResponse);
        }

        // GET: api/Ventas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VentaResponse>> GetVenta(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(dv => dv.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
            {
                return NotFound();
            }

            var ventaResponse = new VentaResponse
            {
                Id = venta.Id,
                NombreCliente = venta.NombreCliente ?? venta.Usuario?.NombreCompleto,
                CorreoCliente = venta.CorreoCliente ?? venta.Usuario?.Correo,
                TelefonoCliente = venta.TelefonoCliente,
                DireccionCliente = venta.DireccionCliente,
                Fecha = venta.Fecha,
                Estado = venta.Estado,
                MetodoPago = venta.MetodoPago,
                Observaciones = venta.Observaciones,
                NumeroFactura = venta.NumeroFactura,
                Subtotal = venta.Subtotal,
                Impuestos = venta.Impuestos,
                Descuento = venta.Descuento,
                Total = venta.Total,
                VendedorNombre = venta.Usuario?.NombreCompleto,
                Productos = venta.DetallesVenta.Select(dv => new DetalleVentaResponse
                {
                    Id = dv.Id,
                    ProductoId = dv.ProductoId,
                    ProductoNombre = dv.Producto?.Nombre,
                    Cantidad = dv.Cantidad,
                    PrecioUnitario = dv.PrecioUnitario,
                    Subtotal = dv.Subtotal
                }).ToList()
            };

            return Ok(ventaResponse);
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

        // PUT: api/Ventas/5/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstadoVenta(int id, [FromBody] ActualizarEstadoVentaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
                return NotFound("Venta no encontrada");

            venta.Estado = request.Estado;
            if (!string.IsNullOrEmpty(request.Observaciones))
                venta.Observaciones = request.Observaciones;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Id = venta.Id,
                Estado = venta.Estado,
                Observaciones = venta.Observaciones,
                Mensaje = $"Estado actualizado a {request.Estado}"
            });
        }

        // GET: api/Ventas/estadisticas
        [HttpGet("estadisticas")]
        public async Task<IActionResult> ObtenerEstadisticasVentas()
        {
            var hoy = DateTime.Today;
            var inicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek);
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

            var estadisticas = new
            {
                VentasHoy = await _context.Ventas.CountAsync(v => v.Fecha.Date == hoy),
                VentasSemana = await _context.Ventas.CountAsync(v => v.Fecha >= inicioSemana),
                VentasMes = await _context.Ventas.CountAsync(v => v.Fecha >= inicioMes),
                
                IngresosHoy = await _context.Ventas
                    .Where(v => v.Fecha.Date == hoy && v.Estado != "Cancelado")
                    .SumAsync(v => (decimal?)v.Total) ?? 0,
                IngresosSemana = await _context.Ventas
                    .Where(v => v.Fecha >= inicioSemana && v.Estado != "Cancelado")
                    .SumAsync(v => (decimal?)v.Total) ?? 0,
                IngresosMes = await _context.Ventas
                    .Where(v => v.Fecha >= inicioMes && v.Estado != "Cancelado")
                    .SumAsync(v => (decimal?)v.Total) ?? 0,

                VentasPorEstado = await _context.Ventas
                    .GroupBy(v => v.Estado)
                    .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                    .ToListAsync(),

                ProductosMasVendidos = await _context.DetalleVentas
                    .Include(dv => dv.Producto)
                    .GroupBy(dv => new { dv.ProductoId, dv.Producto!.Nombre })
                    .Select(g => new
                    {
                        ProductoId = g.Key.ProductoId,
                        ProductoNombre = g.Key.Nombre,
                        CantidadVendida = g.Sum(dv => dv.Cantidad),
                        IngresoTotal = g.Sum(dv => dv.Cantidad * dv.PrecioUnitario)
                    })
                    .OrderByDescending(p => p.CantidadVendida)
                    .Take(5)
                    .ToListAsync()
            };

            return Ok(estadisticas);
        }

        // GET: api/Ventas/por-estado/{estado}
        [HttpGet("por-estado/{estado}")]
        public async Task<ActionResult<IEnumerable<VentaResponse>>> ObtenerVentasPorEstado(string estado)
        {
            var estadosValidos = new[] { "Pendiente", "Procesando", "Enviado", "Entregado", "Cancelado" };
            if (!estadosValidos.Contains(estado))
                return BadRequest("Estado no válido");

            var ventas = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(dv => dv.Producto)
                .Where(v => v.Estado == estado)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            var ventasResponse = ventas.Select(v => new VentaResponse
            {
                Id = v.Id,
                NombreCliente = v.NombreCliente ?? v.Usuario?.NombreCompleto,
                CorreoCliente = v.CorreoCliente ?? v.Usuario?.Correo,
                TelefonoCliente = v.TelefonoCliente,
                DireccionCliente = v.DireccionCliente,
                Fecha = v.Fecha,
                Estado = v.Estado,
                MetodoPago = v.MetodoPago,
                Observaciones = v.Observaciones,
                NumeroFactura = v.NumeroFactura,
                Subtotal = v.Subtotal,
                Impuestos = v.Impuestos,
                Descuento = v.Descuento,
                Total = v.Total,
                VendedorNombre = v.Usuario?.NombreCompleto,
                Productos = v.DetallesVenta.Select(dv => new DetalleVentaResponse
                {
                    Id = dv.Id,
                    ProductoId = dv.ProductoId,
                    ProductoNombre = dv.Producto?.Nombre,
                    Cantidad = dv.Cantidad,
                    PrecioUnitario = dv.PrecioUnitario,
                    Subtotal = dv.Subtotal
                }).ToList()
            }).ToList();

            return Ok(ventasResponse);
        }

        // GET: api/Ventas/buscar?termino=abc
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<VentaResponse>>> BuscarVentas([FromQuery] string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return BadRequest("Debe proporcionar un término de búsqueda");

            var ventas = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(dv => dv.Producto)
                .Where(v => 
                    v.NombreCliente!.Contains(termino) ||
                    v.CorreoCliente!.Contains(termino) ||
                    v.NumeroFactura!.Contains(termino) ||
                    v.Usuario!.NombreCompleto.Contains(termino))
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            var ventasResponse = ventas.Select(v => new VentaResponse
            {
                Id = v.Id,
                NombreCliente = v.NombreCliente ?? v.Usuario?.NombreCompleto,
                CorreoCliente = v.CorreoCliente ?? v.Usuario?.Correo,
                TelefonoCliente = v.TelefonoCliente,
                DireccionCliente = v.DireccionCliente,
                Fecha = v.Fecha,
                Estado = v.Estado,
                MetodoPago = v.MetodoPago,
                Observaciones = v.Observaciones,
                NumeroFactura = v.NumeroFactura,
                Subtotal = v.Subtotal,
                Impuestos = v.Impuestos,
                Descuento = v.Descuento,
                Total = v.Total,
                VendedorNombre = v.Usuario?.NombreCompleto,
                Productos = v.DetallesVenta.Select(dv => new DetalleVentaResponse
                {
                    Id = dv.Id,
                    ProductoId = dv.ProductoId,
                    ProductoNombre = dv.Producto?.Nombre,
                    Cantidad = dv.Cantidad,
                    PrecioUnitario = dv.PrecioUnitario,
                    Subtotal = dv.Subtotal
                }).ToList()
            }).ToList();

            return Ok(ventasResponse);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.Productos == null || !request.Productos.Any())
                return BadRequest("Debe incluir al menos un producto en la venta.");

            // Obtener el usuario actual desde el token JWT
            var usuarioIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (usuarioIdClaim == null)
                return Unauthorized("Usuario no autenticado");

            if (!int.TryParse(usuarioIdClaim.Value, out int usuarioId))
                return BadRequest("ID de usuario inválido");

            // Calcular subtotal
            decimal subtotal = 0;
            var detallesTemporales = new List<DetalleVenta>();

            foreach (var item in request.Productos)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto == null)
                    return NotFound($"Producto con ID {item.ProductoId} no encontrado.");

                var precioUnitario = item.PrecioUnitario ?? producto.PrecioVenta;
                
                var detalle = new DetalleVenta
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = precioUnitario
                };

                detallesTemporales.Add(detalle);
                subtotal += item.Cantidad * precioUnitario;
            }

            // Calcular impuestos (ejemplo: 19% IVA)
            var impuestos = subtotal * 0.19m;
            var total = subtotal - request.Descuento + impuestos;

            // Generar número de factura
            var numeroFactura = $"FAC-{DateTime.Now:yyyyMMdd}-{DateTime.Now.Ticks.ToString().Substring(10)}";

            var venta = new Venta
            {
                UsuarioId = usuarioId,
                NombreCliente = request.NombreCliente,
                CorreoCliente = request.CorreoCliente,
                TelefonoCliente = request.TelefonoCliente,
                DireccionCliente = request.DireccionCliente,
                MetodoPago = request.MetodoPago,
                Observaciones = request.Observaciones,
                Subtotal = subtotal,
                Impuestos = impuestos,
                Descuento = request.Descuento,
                Total = Math.Round(total, 2),
                NumeroFactura = numeroFactura,
                Estado = "Pendiente",
                Fecha = DateTime.Now
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync(); // para obtener venta.Id

            // Agregar detalles de venta
            foreach (var detalle in detallesTemporales)
            {
                detalle.VentaId = venta.Id;
                _context.DetalleVentas.Add(detalle);
            }

            await _context.SaveChangesAsync();

            // Cargar los datos completos para la respuesta
            var ventaCompleta = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(dv => dv.Producto)
                .FirstOrDefaultAsync(v => v.Id == venta.Id);

            var ventaResponse = new VentaResponse
            {
                Id = ventaCompleta!.Id,
                NombreCliente = ventaCompleta.NombreCliente,
                CorreoCliente = ventaCompleta.CorreoCliente,
                TelefonoCliente = ventaCompleta.TelefonoCliente,
                DireccionCliente = ventaCompleta.DireccionCliente,
                Fecha = ventaCompleta.Fecha,
                Estado = ventaCompleta.Estado,
                MetodoPago = ventaCompleta.MetodoPago,
                Observaciones = ventaCompleta.Observaciones,
                NumeroFactura = ventaCompleta.NumeroFactura,
                Subtotal = ventaCompleta.Subtotal,
                Impuestos = ventaCompleta.Impuestos,
                Descuento = ventaCompleta.Descuento,
                Total = ventaCompleta.Total,
                VendedorNombre = ventaCompleta.Usuario?.NombreCompleto,
                Productos = ventaCompleta.DetallesVenta.Select(dv => new DetalleVentaResponse
                {
                    Id = dv.Id,
                    ProductoId = dv.ProductoId,
                    ProductoNombre = dv.Producto?.Nombre,
                    Cantidad = dv.Cantidad,
                    PrecioUnitario = dv.PrecioUnitario,
                    Subtotal = dv.Subtotal
                }).ToList()
            };

            return Ok(ventaResponse);
        }

        [HttpGet("todas")]
        public async Task<IActionResult> ObtenerTodasLasVentas()
        {
            Console.WriteLine("🔧 VentasController - Obteniendo todas las ventas para admin...");
            
            try
            {
                // Primero verificar si hay datos en las tablas
                var ventasCount = await _context.Ventas.CountAsync();
                var usuariosCount = await _context.Usuarios.CountAsync();
                var detallesCount = await _context.DetalleVentas.CountAsync();
                var productosCount = await _context.Productos.CountAsync();
                
                Console.WriteLine($"🔧 Conteos: Ventas={ventasCount}, Usuarios={usuariosCount}, DetalleVentas={detallesCount}, Productos={productosCount}");
                
                // Si no hay ventas, devolver lista vacía
                if (ventasCount == 0)
                {
                    Console.WriteLine("🔧 No hay ventas en la base de datos");
                    return Ok(new List<object>());
                }
                
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
                                      total = venta.Subtotal + venta.Impuestos - venta.Descuento, // Calcular total en lugar de usar venta.Total
                                      estado = venta.Estado ?? "Completada", 
                                      fecha = venta.Fecha
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
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Error en ObtenerTodasLasVentas: {ex.Message}");
                Console.WriteLine($"🚨 Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = "Error interno del servidor", message = ex.Message });
            }
        }

        // Endpoint temporal sin autenticación para debugging
        [HttpGet("test-conexion")]
        public async Task<IActionResult> TestConexion()
        {
            Console.WriteLine("🔧 VentasController - Test de conexión iniciado...");
            
            try
            {
                var ventasCount = await _context.Ventas.CountAsync();
                Console.WriteLine($"🔧 Test exitoso - Ventas encontradas: {ventasCount}");
                
                return Ok(new { 
                    message = "Conexión exitosa", 
                    ventasCount = ventasCount,
                    timestamp = DateTime.Now 
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Error en test de conexión: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
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
