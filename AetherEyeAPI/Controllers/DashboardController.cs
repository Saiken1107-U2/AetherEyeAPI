using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AetherEyeAPI.Data;

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
    }
}
