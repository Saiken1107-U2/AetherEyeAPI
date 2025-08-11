using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AetherEyeAPI.Data;
using AetherEyeAPI.Models;

namespace AetherEyeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public DashboardController(AetherEyeDbContext context)
        {
            _context = context;
        }

        [HttpGet("resumen")]
        public async Task<IActionResult> ObtenerResumen()
        {
            var totalVentas = await _context.Ventas.SumAsync(v => (decimal?)v.Total) ?? 0;
            var totalCotizaciones = await _context.Cotizaciones.CountAsync();
            var totalUsuarios = await _context.Usuarios.CountAsync();
            var totalProductos = await _context.Productos.CountAsync();
            var totalComentarios = await _context.Comentarios.CountAsync();
            var insumosBajoStock = await _context.Insumos
                .Where(i => i.StockActual < 5)
                .Select(i => new { i.Nombre, i.StockActual })
                .ToListAsync();

            return Ok(new
            {
                TotalVentas = Math.Round(totalVentas, 2),
                TotalCotizaciones = totalCotizaciones,
                TotalUsuarios = totalUsuarios,
                TotalProductos = totalProductos,
                TotalComentarios = totalComentarios,
                InsumosBajoStock = insumosBajoStock
            });
        }

        [HttpGet("totales")]
        public async Task<IActionResult> ObtenerTotales()
        {
            var usuarios = await _context.Usuarios.CountAsync();
            var productos = await _context.Productos.CountAsync();
            var ventas = await _context.Ventas.CountAsync();
            var comentarios = await _context.Comentarios.CountAsync();

            return Ok(new { usuarios, productos, ventas, comentarios });
        }

        // Nuevos endpoints para Dashboard avanzado

        // GET: api/Dashboard/stats
        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStats>> GetDashboardStats()
        {
            try
            {
                var today = DateTime.Today;
                
                var stats = new DashboardStats
                {
                    TotalInsumos = await _context.Insumos.CountAsync(),
                    ValorTotalInventario = await _context.Insumos
                        .Where(i => i.StockActual.HasValue && i.CostoUnitario.HasValue)
                        .SumAsync(i => i.StockActual.Value * i.CostoUnitario.Value),
                    InsumosStockBajo = await _context.Insumos
                        .Where(i => i.StockActual.HasValue && i.StockMinimo.HasValue && 
                                   i.StockActual <= i.StockMinimo)
                        .CountAsync(),
                    MovimientosHoy = await _context.MovimientosStock
                        .Where(m => m.Fecha.Date == today)
                        .CountAsync(),
                    TotalProveedores = await _context.Proveedores
                        .Where(p => p.EstaActivo)
                        .CountAsync()
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // GET: api/Dashboard/stock-by-category
        [HttpGet("stock-by-category")]
        public async Task<ActionResult<IEnumerable<StockByCategory>>> GetStockByCategory()
        {
            try
            {
                var stockByCategory = await _context.Insumos
                    .Where(i => !string.IsNullOrEmpty(i.Categoria))
                    .GroupBy(i => i.Categoria)
                    .Select(g => new StockByCategory
                    {
                        Categoria = g.Key,
                        Cantidad = g.Count(),
                        ValorTotal = g.Where(i => i.StockActual.HasValue && i.CostoUnitario.HasValue)
                                    .Sum(i => i.StockActual.Value * i.CostoUnitario.Value)
                    })
                    .OrderByDescending(s => s.Cantidad)
                    .ToListAsync();

                return Ok(stockByCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // GET: api/Dashboard/movements/{days}
        [HttpGet("movements/{days:int}")]
        public async Task<ActionResult<IEnumerable<MovementTrend>>> GetMovementTrend(int days = 7)
        {
            try
            {
                var startDate = DateTime.Today.AddDays(-days);
                
                var movements = await _context.MovimientosStock
                    .Where(m => m.Fecha >= startDate)
                    .GroupBy(m => m.Fecha.Date)
                    .Select(g => new MovementTrend
                    {
                        Fecha = g.Key,
                        Entradas = g.Count(m => m.TipoMovimiento == "ENTRADA"),
                        Salidas = g.Count(m => m.TipoMovimiento == "SALIDA"),
                        Ajustes = g.Count(m => m.TipoMovimiento == "AJUSTE")
                    })
                    .OrderBy(m => m.Fecha)
                    .ToListAsync();

                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // GET: api/Dashboard/low-stock-alerts
        [HttpGet("low-stock-alerts")]
        public async Task<ActionResult<IEnumerable<AlertItem>>> GetLowStockAlerts()
        {
            try
            {
                var alerts = await _context.Insumos
                    .Where(i => i.StockActual.HasValue && i.StockMinimo.HasValue && 
                               i.StockActual <= i.StockMinimo)
                    .Select(i => new AlertItem
                    {
                        InsumoId = i.Id,
                        Nombre = i.Nombre,
                        StockActual = i.StockActual.Value,
                        StockMinimo = i.StockMinimo.Value,
                        Categoria = i.Categoria ?? "Sin categoría",
                        Urgencia = i.StockActual == 0 ? "Crítico" : 
                                  i.StockActual <= (i.StockMinimo * 0.5m) ? "Alto" : "Medio"
                    })
                    .OrderBy(a => a.StockActual)
                    .ToListAsync();

                return Ok(alerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // GET: api/Dashboard/top-items/{count}
        [HttpGet("top-items/{count:int}")]
        public async Task<ActionResult<IEnumerable<TopItem>>> GetTopItems(int count = 5)
        {
            try
            {
                var topItems = await _context.MovimientosStock
                    .Include(m => m.Insumo)
                    .GroupBy(m => new { m.InsumoId, m.Insumo!.Nombre, m.Insumo.Categoria })
                    .Select(g => new TopItem
                    {
                        InsumoId = g.Key.InsumoId,
                        Nombre = g.Key.Nombre,
                        Categoria = g.Key.Categoria ?? "Sin categoría",
                        TotalMovimientos = g.Count(),
                        CantidadTotal = g.Sum(m => m.Cantidad)
                    })
                    .OrderByDescending(t => t.TotalMovimientos)
                    .Take(count)
                    .ToListAsync();

                return Ok(topItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}
