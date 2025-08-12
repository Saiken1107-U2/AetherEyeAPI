using System;
using System.Collections.Generic;
using System.Data;
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
        public async Task<ActionResult<IEnumerable<object>>> GetCompras()
        {
            // Consulta base simplificada sin Include/ThenInclude
            var comprasData = await _context.Compras
                .Select(c => new {
                    c.Id,
                    c.Fecha,
                    c.Total,
                    c.ProveedorId,
                    c.Estado,
                    c.NumeroFactura,
                    c.Observaciones
                })
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();

            var result = new List<object>();

            foreach (var compra in comprasData)
            {
                // Obtener proveedor por separado
                var proveedor = await _context.Proveedores
                    .Where(p => p.Id == compra.ProveedorId)
                    .Select(p => new {
                        p.Id,
                        p.Nombre,
                        p.NombreContacto
                    })
                    .FirstOrDefaultAsync();

                // Obtener detalles por separado
                var detalles = await _context.DetalleCompras
                    .Where(dc => dc.CompraId == compra.Id)
                    .Select(dc => new {
                        dc.Id,
                        dc.Cantidad,
                        dc.CostoUnitario,
                        Subtotal = dc.Cantidad * dc.CostoUnitario,
                        dc.InsumoId
                    })
                    .ToListAsync();

                var detallesConInsumos = new List<object>();
                foreach (var detalle in detalles)
                {
                    var insumo = await _context.Insumos
                        .Where(i => i.Id == detalle.InsumoId)
                        .Select(i => new {
                            i.Id,
                            i.Nombre,
                            i.UnidadMedida
                        })
                        .FirstOrDefaultAsync();

                    detallesConInsumos.Add(new {
                        detalle.Id,
                        detalle.Cantidad,
                        detalle.CostoUnitario,
                        detalle.Subtotal,
                        Insumo = insumo
                    });
                }

                result.Add(new {
                    compra.Id,
                    compra.Fecha,
                    compra.Total,
                    Proveedor = proveedor,
                    DetallesCompra = detallesConInsumos,
                    TotalItems = detalles.Count,
                    compra.Estado,
                    compra.NumeroFactura,
                    compra.Observaciones
                });
            }

            return Ok(result);
        }

        // GET: api/Compras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCompra(int id)
        {
            // Obtener la compra básica
            var compraBasica = await _context.Compras
                .Where(c => c.Id == id)
                .Select(c => new {
                    c.Id,
                    c.Fecha,
                    c.Total,
                    c.ProveedorId,
                    c.Estado,
                    c.NumeroFactura,
                    c.Observaciones
                })
                .FirstOrDefaultAsync();

            if (compraBasica == null)
            {
                return NotFound();
            }

            // Obtener el proveedor por separado
            var proveedor = await _context.Proveedores
                .Where(p => p.Id == compraBasica.ProveedorId)
                .Select(p => new {
                    p.Id,
                    p.Nombre,
                    p.NombreContacto,
                    p.Correo,
                    p.Telefono
                })
                .FirstOrDefaultAsync();

            // Obtener los detalles de compra con insumos
            var detallesCompra = await _context.DetalleCompras
                .Where(dc => dc.CompraId == id)
                .Select(dc => new {
                    dc.Id,
                    dc.Cantidad,
                    dc.CostoUnitario,
                    Subtotal = dc.Cantidad * dc.CostoUnitario,
                    dc.InsumoId
                })
                .ToListAsync();

            // Obtener los insumos relacionados
            var insumoIds = detallesCompra.Select(dc => dc.InsumoId).ToList();
            var insumos = await _context.Insumos
                .Where(i => insumoIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id, i => new {
                    i.Id,
                    i.Nombre,
                    i.UnidadMedida,
                    i.Categoria
                });

            // Combinar los datos
            var compra = new {
                compraBasica.Id,
                compraBasica.Fecha,
                compraBasica.Total,
                compraBasica.Estado,
                compraBasica.NumeroFactura,
                compraBasica.Observaciones,
                Proveedor = proveedor,
                DetallesCompra = detallesCompra.Select(dc => new {
                    dc.Id,
                    dc.Cantidad,
                    dc.CostoUnitario,
                    dc.Subtotal,
                    Insumo = insumos.ContainsKey(dc.InsumoId) ? insumos[dc.InsumoId] : null
                }).ToList()
            };

            return Ok(compra);
        }

        // GET: api/Compras/Estadisticas
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticasCompras()
        {
            var fechaInicio = DateTime.Now.AddDays(-30);
            
            // Consultas básicas y simples
            var totalComprasDelMes = await _context.Compras
                .Where(c => c.Fecha >= fechaInicio)
                .CountAsync();
                
            var montoTotalDelMes = await _context.Compras
                .Where(c => c.Fecha >= fechaInicio)
                .SumAsync(c => (decimal?)c.Total) ?? 0;
                
            var promedioCompra = totalComprasDelMes > 0 
                ? montoTotalDelMes / totalComprasDelMes 
                : 0;
            
            // Proveedor más activo - consulta completamente simplificada
            var comprasDelPeriodoProveedores = await _context.Compras
                .Where(c => c.Fecha >= fechaInicio)
                .Select(c => new { c.ProveedorId, c.Total })
                .ToListAsync();

            var proveedorMasActivoData = comprasDelPeriodoProveedores
                .GroupBy(c => c.ProveedorId)
                .Select(g => new {
                    ProveedorId = g.Key,
                    TotalCompras = g.Count(),
                    MontoTotal = g.Sum(c => c.Total)
                })
                .OrderByDescending(x => x.TotalCompras)
                .FirstOrDefault();

            object? proveedorMasActivo = null;
            if (proveedorMasActivoData != null)
            {
                var proveedor = await _context.Proveedores
                    .FindAsync(proveedorMasActivoData.ProveedorId);
                
                proveedorMasActivo = new {
                    ProveedorId = proveedorMasActivoData.ProveedorId,
                    NombreProveedor = proveedor?.Nombre ?? "Desconocido",
                    TotalCompras = proveedorMasActivoData.TotalCompras,
                    MontoTotal = proveedorMasActivoData.MontoTotal
                };
            }
            
            // Insumo más comprado - consulta completamente simplificada
            var comprasDelPeriodoIds = await _context.Compras
                .Where(c => c.Fecha >= fechaInicio)
                .Select(c => c.Id)
                .ToListAsync();

            // Obtener todos los detalles del período
            var detallesDelPeriodo = await _context.DetalleCompras
                .Where(dc => comprasDelPeriodoIds.Contains(dc.CompraId))
                .Select(dc => new {
                    dc.InsumoId,
                    dc.Cantidad,
                    Subtotal = dc.Cantidad * dc.CostoUnitario
                })
                .ToListAsync();

            // Agrupar manualmente en memoria
            var insumoMasCompradoData = detallesDelPeriodo
                .GroupBy(dc => dc.InsumoId)
                .Select(g => new {
                    InsumoId = g.Key,
                    CantidadTotal = g.Sum(dc => dc.Cantidad),
                    MontoTotal = g.Sum(dc => dc.Subtotal)
                })
                .OrderByDescending(x => x.CantidadTotal)
                .FirstOrDefault();

            object? insumoMasComprado = null;
            if (insumoMasCompradoData != null)
            {
                var insumo = await _context.Insumos
                    .FindAsync(insumoMasCompradoData.InsumoId);
                
                insumoMasComprado = new {
                    InsumoId = insumoMasCompradoData.InsumoId,
                    NombreInsumo = insumo?.Nombre ?? "Desconocido",
                    CantidadTotal = insumoMasCompradoData.CantidadTotal,
                    MontoTotal = insumoMasCompradoData.MontoTotal
                };
            }
            
            // Compras por semana - consulta simplificada
            var comprasDelPeriodo = await _context.Compras
                .Where(c => c.Fecha >= fechaInicio)
                .Select(c => new { c.Fecha, c.Total })
                .ToListAsync();
                
            var comprasPorSemana = comprasDelPeriodo
                .GroupBy(c => new { 
                    Semana = (c.Fecha.DayOfYear - 1) / 7 + 1,
                    Año = c.Fecha.Year 
                })
                .Select(g => new {
                    Semana = g.Key.Semana,
                    Año = g.Key.Año,
                    CantidadCompras = g.Count(),
                    MontoTotal = g.Sum(c => c.Total)
                })
                .OrderBy(x => x.Año).ThenBy(x => x.Semana)
                .ToList();

            var estadisticas = new {
                TotalComprasDelMes = totalComprasDelMes,
                MontoTotalDelMes = montoTotalDelMes,
                PromedioCompra = promedioCompra,
                ProveedorMasActivo = proveedorMasActivo,
                InsumoMasComprado = insumoMasComprado,
                ComprasPorSemana = comprasPorSemana
            };

            return Ok(estadisticas);
        }

        // PUT: api/Compras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompra(int id, CompraRequest request)
        {
            try
            {
                // Verificar que la compra existe
                var compraExistente = await _context.Compras
                    .Include(c => c.DetallesCompra)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (compraExistente == null)
                {
                    return NotFound($"No se encontró la compra con ID {id}");
                }

                // Verificar que el proveedor existe
                var proveedorExiste = await _context.Proveedores
                    .AnyAsync(p => p.Id == request.ProveedorId);

                if (!proveedorExiste)
                {
                    return BadRequest($"El proveedor con ID {request.ProveedorId} no existe");
                }

                // Actualizar datos de la compra
                compraExistente.ProveedorId = request.ProveedorId;
                compraExistente.NumeroFactura = request.NumeroFactura;
                compraExistente.Observaciones = request.Observaciones;

                // Eliminar detalles existentes
                _context.DetalleCompras.RemoveRange(compraExistente.DetallesCompra);

                // Agregar nuevos detalles y calcular el total
                decimal nuevoTotal = 0;
                foreach (var insumoRequest in request.Insumos)
                {
                    var insumoExiste = await _context.Insumos
                        .AnyAsync(i => i.Id == insumoRequest.InsumoId);

                    if (!insumoExiste)
                    {
                        return BadRequest($"El insumo con ID {insumoRequest.InsumoId} no existe");
                    }

                    var detalleCompra = new DetalleCompra
                    {
                        CompraId = id,
                        InsumoId = insumoRequest.InsumoId,
                        Cantidad = insumoRequest.Cantidad,
                        CostoUnitario = insumoRequest.CostoUnitario
                    };

                    _context.DetalleCompras.Add(detalleCompra);
                    nuevoTotal += insumoRequest.Cantidad * insumoRequest.CostoUnitario;
                }

                // Actualizar el total
                compraExistente.Total = nuevoTotal;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Mensaje = "Compra actualizada exitosamente",
                    CompraId = id,
                    Total = nuevoTotal
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error interno del servidor",
                    Error = ex.Message
                });
            }
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
            try
            {
                // Verificar que la compra existe
                var compra = await _context.Compras
                    .Include(c => c.DetallesCompra)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (compra == null)
                {
                    return NotFound($"No se encontró la compra con ID {id}");
                }

                // Eliminar primero los detalles (por integridad referencial)
                _context.DetalleCompras.RemoveRange(compra.DetallesCompra);

                // Eliminar la compra
                _context.Compras.Remove(compra);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Mensaje = "Compra eliminada exitosamente",
                    CompraId = id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error interno del servidor",
                    Error = ex.Message
                });
            }
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
                Fecha = DateTime.Now,
                Estado = "Completada", // Estado por defecto
                NumeroFactura = request.NumeroFactura,
                Observaciones = request.Observaciones
            };

            _context.Compras.Add(compra);
            await _context.SaveChangesAsync(); // Guardamos para obtener el ID

            foreach (var item in request.Insumos)
            {
                var insumo = await _context.Insumos.FindAsync(item.InsumoId);
                if (insumo == null)
                    return NotFound($"Insumo con ID {item.InsumoId} no encontrado.");

                // Cálculo de nuevo costo promedio
                decimal stockAnterior = insumo.StockActual ?? 0;
                decimal costoAnterior = insumo.CostoUnitario ?? 0;
                decimal cantidadNueva = item.Cantidad;
                decimal costoNuevo = item.CostoUnitario;

                decimal nuevoStock = stockAnterior + cantidadNueva;

                decimal nuevoCostoPromedio = nuevoStock == 0
                    ? costoNuevo
                    : ((stockAnterior * costoAnterior) + (cantidadNueva * costoNuevo)) / nuevoStock;

                // Actualizar insumo
                insumo.StockActual = nuevoStock;
                insumo.CostoUnitario = Math.Round(nuevoCostoPromedio, 2);
                insumo.FechaActualizacion = DateTime.UtcNow;

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