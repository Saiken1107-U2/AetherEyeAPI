using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AetherEyeAPI.Data;
using AetherEyeAPI.Models;

namespace AetherEyeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetasController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public RecetasController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/Recetas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receta>>> GetRecetas()
        {
            try
            {
                var recetas = await _context.Recetas
                    .Include(r => r.Producto)
                    .Include(r => r.Insumo)
                    .ToListAsync();

                return Ok(recetas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener recetas: {ex.Message}");
            }
        }

        // GET: api/Recetas/producto/5
        [HttpGet("producto/{productoId}")]
        public async Task<ActionResult<IEnumerable<Receta>>> GetRecetasPorProducto(int productoId)
        {
            try
            {
                var recetas = await _context.Recetas
                    .Include(r => r.Producto)
                    .Include(r => r.Insumo)
                    .Where(r => r.ProductoId == productoId)
                    .ToListAsync();

                return Ok(recetas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener recetas del producto: {ex.Message}");
            }
        }

        // GET: api/Recetas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Receta>> GetReceta(int id)
        {
            try
            {
                var receta = await _context.Recetas
                    .Include(r => r.Producto)
                    .Include(r => r.Insumo)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (receta == null)
                {
                    return NotFound();
                }

                return Ok(receta);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener receta: {ex.Message}");
            }
        }

        // PUT: api/Recetas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReceta(int id, Receta receta)
        {
            if (id != receta.Id)
            {
                return BadRequest();
            }

            // Validaciones para sistema IoT
            if (receta.ProductoId <= 0)
            {
                return BadRequest("ProductoId es requerido y debe ser mayor a 0");
            }

            if (receta.InsumoId <= 0)
            {
                return BadRequest("InsumoId es requerido y debe ser mayor a 0");
            }

            if (receta.CantidadNecesaria <= 0)
            {
                return BadRequest("CantidadNecesaria debe ser mayor a 0");
            }

            if (string.IsNullOrWhiteSpace(receta.UnidadMedida))
            {
                return BadRequest("UnidadMedida es requerida");
            }

            try
            {
                // Verificar que el producto existe
                var productoExiste = await _context.Productos.AnyAsync(p => p.Id == receta.ProductoId);
                if (!productoExiste)
                {
                    return BadRequest("El producto especificado no existe");
                }

                // Verificar que el insumo existe
                var insumoExiste = await _context.Insumos.AnyAsync(i => i.Id == receta.InsumoId);
                if (!insumoExiste)
                {
                    return BadRequest("El insumo especificado no existe");
                }

                // Verificar duplicados (excluyendo el registro actual)
                var existeReceta = await _context.Recetas.AnyAsync(r => 
                    r.Id != id &&
                    r.ProductoId == receta.ProductoId && 
                    r.InsumoId == receta.InsumoId);
                
                if (existeReceta)
                {
                    return BadRequest("Ya existe una receta con la misma combinación de producto e insumo");
                }

                _context.Entry(receta).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecetaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar receta: {ex.Message}");
            }
        }

        // POST: api/Recetas
        [HttpPost]
        public async Task<ActionResult<Receta>> PostReceta(Receta receta)
        {
            // Validaciones básicas para sistema IoT
            if (receta.ProductoId <= 0)
            {
                return BadRequest("ProductoId es requerido y debe ser mayor a 0");
            }

            if (receta.InsumoId <= 0)
            {
                return BadRequest("InsumoId es requerido y debe ser mayor a 0");
            }

            if (receta.CantidadNecesaria <= 0)
            {
                return BadRequest("CantidadNecesaria debe ser mayor a 0");
            }

            if (string.IsNullOrWhiteSpace(receta.UnidadMedida))
            {
                return BadRequest("UnidadMedida es requerida");
            }

            try
            {
                // Verificar que el producto existe
                var productoExiste = await _context.Productos.AnyAsync(p => p.Id == receta.ProductoId);
                if (!productoExiste)
                {
                    return BadRequest("El producto especificado no existe");
                }

                // Verificar que el insumo existe
                var insumoExiste = await _context.Insumos.AnyAsync(i => i.Id == receta.InsumoId);
                if (!insumoExiste)
                {
                    return BadRequest("El insumo especificado no existe");
                }

                // Verificar duplicados
                var existeReceta = await _context.Recetas.AnyAsync(r => 
                    r.ProductoId == receta.ProductoId && 
                    r.InsumoId == receta.InsumoId);
                
                if (existeReceta)
                {
                    return BadRequest("Ya existe una receta con la misma combinación de producto e insumo");
                }

                // Asignar valores por defecto
                receta.FechaCreacion = DateTime.Now;
                
                _context.Recetas.Add(receta);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetReceta", new { id = receta.Id }, receta);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear la receta: {ex.Message}");
            }
        }

        // DELETE: api/Recetas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceta(int id)
        {
            try
            {
                var receta = await _context.Recetas.FindAsync(id);
                if (receta == null)
                {
                    return NotFound();
                }

                _context.Recetas.Remove(receta);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar receta: {ex.Message}");
            }
        }

        // POST: api/Recetas/explosion
        [HttpPost("explosion")]
        public async Task<IActionResult> CalcularExplosionMateriales([FromBody] dynamic request)
        {
            try
            {
                // Extraer valores del request dinámico
                int productoId = request.productoId;
                int cantidadProductos = request.cantidadProductos ?? 1;
                bool verificarStock = request.verificarStock ?? true;

                var producto = await _context.Productos.FindAsync(productoId);
                if (producto == null)
                {
                    return NotFound("Producto no encontrado");
                }

                var recetas = await _context.Recetas
                    .Include(r => r.Insumo)
                    .Where(r => r.ProductoId == productoId)
                    .ToListAsync();

                var items = new List<object>();
                decimal costoTotal = 0;

                foreach (var receta in recetas)
                {
                    var cantidadNecesaria = receta.CantidadNecesaria * cantidadProductos;
                    var stockActual = verificarStock ? (receta.Insumo?.StockActual ?? 0) : 0;
                    var tieneFaltante = verificarStock && stockActual < cantidadNecesaria;
                    var costoItem = cantidadNecesaria * (receta.Insumo?.CostoUnitario ?? 0);

                    var item = new
                    {
                        insumoId = receta.InsumoId,
                        nombre = receta.Insumo?.Nombre ?? "Insumo no encontrado",
                        tipo = "Insumo",
                        cantidadUnitaria = receta.CantidadNecesaria,
                        cantidadTotal = cantidadNecesaria,
                        cantidadConMerma = cantidadNecesaria,
                        unidadMedida = receta.UnidadMedida,
                        costoUnitario = receta.Insumo?.CostoUnitario ?? 0,
                        costoTotal = costoItem,
                        porcentajeMerma = 0,
                        nivel = 0,
                        stockActual = (int)stockActual,
                        stockDisponible = (int)(stockActual > cantidadNecesaria ? stockActual - cantidadNecesaria : 0),
                        tieneFaltante = tieneFaltante,
                        esCritico = false,
                        subItems = new List<object>()
                    };

                    items.Add(item);
                    costoTotal += costoItem;
                }

                var resultado = new
                {
                    productoId = productoId,
                    productoNombre = producto.Nombre,
                    cantidadProductos = cantidadProductos,
                    items = items,
                    costoTotalMateriales = costoTotal,
                    costoTotalConMerma = costoTotal,
                    tieneFaltantes = items.Any(i => ((dynamic)i).tieneFaltante),
                    alertasFaltantes = items.Where(i => ((dynamic)i).tieneFaltante)
                        .Select(i => $"Falta stock para {((dynamic)i).nombre}: necesario {((dynamic)i).cantidadTotal}, disponible {((dynamic)i).stockActual}")
                        .ToList()
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al calcular explosión de materiales: {ex.Message}");
            }
        }

        // GET: api/Recetas/solicitud-compra/{productoId}/{cantidad}
        [HttpGet("solicitud-compra/{productoId}/{cantidad}")]
        public async Task<IActionResult> GenerarSolicitudCompra(int productoId, int cantidad)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(productoId);
                if (producto == null)
                {
                    return NotFound("Producto no encontrado");
                }

                var recetas = await _context.Recetas
                    .Include(r => r.Insumo)
                    .Where(r => r.ProductoId == productoId)
                    .ToListAsync();

                var itemsFaltantes = new List<object>();
                decimal montoEstimado = 0;

                foreach (var receta in recetas)
                {
                    var cantidadNecesaria = receta.CantidadNecesaria * cantidad;
                    var stockActual = receta.Insumo?.StockActual ?? 0;
                    
                    if (stockActual < cantidadNecesaria)
                    {
                        var cantidadFaltante = cantidadNecesaria - stockActual;
                        var costoUnitario = receta.Insumo?.CostoUnitario ?? 0;
                        var montoTotal = cantidadFaltante * costoUnitario;

                        var item = new
                        {
                            insumoId = receta.InsumoId,
                            insumoNombre = receta.Insumo?.Nombre ?? "Insumo no encontrado",
                            cantidadFaltante = cantidadFaltante,
                            unidadMedida = receta.UnidadMedida,
                            costoUnitarioEstimado = costoUnitario,
                            montoTotal = montoTotal,
                            esCritico = false
                        };

                        itemsFaltantes.Add(item);
                        montoEstimado += montoTotal;
                    }
                }

                var solicitud = new
                {
                    productoObjetivo = productoId,
                    cantidadObjetivo = cantidad,
                    items = itemsFaltantes,
                    montoEstimado = montoEstimado,
                    fechaNecesaria = DateTime.Now.AddDays(7)
                };

                return Ok(solicitud);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al generar solicitud de compra: {ex.Message}");
            }
        }

        private bool RecetaExists(int id)
        {
            return _context.Recetas.Any(e => e.Id == id);
        }
    }
}