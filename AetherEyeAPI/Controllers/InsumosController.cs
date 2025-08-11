using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AetherEyeAPI.Data;
using AetherEyeAPI.Models;

namespace AetherEyeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsumosController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public InsumosController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/insumos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetInsumos()
        {
            try
            {
                var insumos = await _context.Insumos
                    .Include(i => i.Proveedor)
                    .Select(i => new
                    {
                        i.Id,
                        i.Nombre,
                        i.Descripcion,
                        i.CodigoInterno,
                        i.Categoria,
                        i.UnidadMedida,
                        i.CostoUnitario,
                        i.StockActual,
                        i.StockMinimo,
                        i.StockMaximo,
                        i.ProveedorId,
                        ProveedorNombre = i.Proveedor != null ? i.Proveedor.Nombre : null,
                        i.FechaVencimiento,
                        i.FechaCreacion,
                        i.FechaActualizacion
                    })
                    .OrderBy(i => i.Nombre)
                    .ToListAsync();

                return Ok(insumos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener los insumos", error = ex.Message });
            }
        }

        // GET: api/insumos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetInsumo(int id)
        {
            try
            {
                var insumo = await _context.Insumos
                    .Include(i => i.Proveedor)
                    .Where(i => i.Id == id)
                    .Select(i => new
                    {
                        i.Id,
                        i.Nombre,
                        i.Descripcion,
                        i.CodigoInterno,
                        i.Categoria,
                        i.UnidadMedida,
                        i.CostoUnitario,
                        i.StockActual,
                        i.StockMinimo,
                        i.StockMaximo,
                        i.ProveedorId,
                        ProveedorNombre = i.Proveedor != null ? i.Proveedor.Nombre : null,
                        i.FechaVencimiento,
                        i.FechaCreacion,
                        i.FechaActualizacion
                    })
                    .FirstOrDefaultAsync();

                if (insumo == null)
                {
                    return NotFound(new { message = "Insumo no encontrado" });
                }

                return Ok(insumo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el insumo", error = ex.Message });
            }
        }

        // GET: api/insumos/categorias
        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategorias()
        {
            try
            {
                var categorias = await _context.Insumos
                    .Where(i => !string.IsNullOrEmpty(i.Categoria))
                    .Select(i => i.Categoria)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener las categorías", error = ex.Message });
            }
        }

        // POST: api/insumos
        [HttpPost]
        public async Task<ActionResult<object>> CreateInsumo([FromBody] InsumoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si ya existe un insumo con el mismo nombre
                var existeInsumo = await _context.Insumos
                    .AnyAsync(i => i.Nombre.ToLower() == request.Nombre.ToLower());

                if (existeInsumo)
                {
                    return BadRequest(new { message = "Ya existe un insumo con ese nombre" });
                }

                // Verificar si el proveedor existe (si se proporciona)
                if (request.ProveedorId.HasValue)
                {
                    var proveedorExiste = await _context.Proveedores
                        .AnyAsync(p => p.Id == request.ProveedorId.Value);

                    if (!proveedorExiste)
                    {
                        return BadRequest(new { message = "El proveedor especificado no existe" });
                    }
                }

                var insumo = new Insumo
                {
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    CodigoInterno = request.CodigoInterno,
                    Categoria = request.Categoria,
                    UnidadMedida = request.UnidadMedida,
                    CostoUnitario = request.CostoUnitario,
                    StockActual = request.StockActual,
                    StockMinimo = request.StockMinimo,
                    StockMaximo = request.StockMaximo,
                    ProveedorId = request.ProveedorId,
                    FechaVencimiento = request.FechaVencimiento,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow
                };

                _context.Insumos.Add(insumo);
                await _context.SaveChangesAsync();

                // Cargar el insumo con el proveedor para la respuesta
                var insumoCreado = await _context.Insumos
                    .Include(i => i.Proveedor)
                    .Where(i => i.Id == insumo.Id)
                    .Select(i => new
                    {
                        i.Id,
                        i.Nombre,
                        i.Descripcion,
                        i.CodigoInterno,
                        i.Categoria,
                        i.UnidadMedida,
                        i.CostoUnitario,
                        i.StockActual,
                        i.StockMinimo,
                        i.StockMaximo,
                        i.ProveedorId,
                        ProveedorNombre = i.Proveedor != null ? i.Proveedor.Nombre : null,
                        i.FechaVencimiento,
                        i.FechaCreacion,
                        i.FechaActualizacion
                    })
                    .FirstOrDefaultAsync();

                return CreatedAtAction(nameof(GetInsumo), new { id = insumo.Id }, insumoCreado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear el insumo", error = ex.Message });
            }
        }

        // PUT: api/insumos/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> UpdateInsumo(int id, [FromBody] InsumoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var insumo = await _context.Insumos.FindAsync(id);
                if (insumo == null)
                {
                    return NotFound(new { message = "Insumo no encontrado" });
                }

                // Verificar si ya existe otro insumo con el mismo nombre
                var existeOtroInsumo = await _context.Insumos
                    .AnyAsync(i => i.Id != id && i.Nombre.ToLower() == request.Nombre.ToLower());

                if (existeOtroInsumo)
                {
                    return BadRequest(new { message = "Ya existe otro insumo con ese nombre" });
                }

                // Verificar si el proveedor existe (si se proporciona)
                if (request.ProveedorId.HasValue)
                {
                    var proveedorExiste = await _context.Proveedores
                        .AnyAsync(p => p.Id == request.ProveedorId.Value);

                    if (!proveedorExiste)
                    {
                        return BadRequest(new { message = "El proveedor especificado no existe" });
                    }
                }

                // Actualizar propiedades
                insumo.Nombre = request.Nombre;
                insumo.Descripcion = request.Descripcion;
                insumo.CodigoInterno = request.CodigoInterno;
                insumo.Categoria = request.Categoria;
                insumo.UnidadMedida = request.UnidadMedida;
                insumo.CostoUnitario = request.CostoUnitario;
                insumo.StockActual = request.StockActual;
                insumo.StockMinimo = request.StockMinimo;
                insumo.StockMaximo = request.StockMaximo;
                insumo.ProveedorId = request.ProveedorId;
                insumo.FechaVencimiento = request.FechaVencimiento;
                insumo.FechaActualizacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Cargar el insumo actualizado con el proveedor
                var insumoActualizado = await _context.Insumos
                    .Include(i => i.Proveedor)
                    .Where(i => i.Id == id)
                    .Select(i => new
                    {
                        i.Id,
                        i.Nombre,
                        i.Descripcion,
                        i.CodigoInterno,
                        i.Categoria,
                        i.UnidadMedida,
                        i.CostoUnitario,
                        i.StockActual,
                        i.StockMinimo,
                        i.StockMaximo,
                        i.ProveedorId,
                        ProveedorNombre = i.Proveedor != null ? i.Proveedor.Nombre : null,
                        i.FechaVencimiento,
                        i.FechaCreacion,
                        i.FechaActualizacion
                    })
                    .FirstOrDefaultAsync();

                return Ok(insumoActualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el insumo", error = ex.Message });
            }
        }

        // DELETE: api/insumos/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInsumo(int id)
        {
            try
            {
                var insumo = await _context.Insumos.FindAsync(id);
                if (insumo == null)
                {
                    return NotFound(new { message = "Insumo no encontrado" });
                }

                _context.Insumos.Remove(insumo);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Insumo eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el insumo", error = ex.Message });
            }
        }

        // POST: api/insumos/{id}/ajustar-stock
        [HttpPost("{id}/ajustar-stock")]
        public async Task<ActionResult> AjustarStock(int id, [FromBody] MovimientoStockRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var insumo = await _context.Insumos.FindAsync(id);
                if (insumo == null)
                {
                    return NotFound(new { message = "Insumo no encontrado" });
                }

                var stockAnterior = insumo.StockActual ?? 0;
                decimal stockNuevo = stockAnterior;

                // Calcular nuevo stock según el tipo de movimiento
                switch (request.TipoMovimiento.ToUpper())
                {
                    case "ENTRADA":
                        stockNuevo = stockAnterior + request.Cantidad;
                        break;
                    case "SALIDA":
                        stockNuevo = stockAnterior - request.Cantidad;
                        if (stockNuevo < 0)
                        {
                            return BadRequest(new { message = "El stock no puede ser negativo" });
                        }
                        break;
                    case "AJUSTE":
                        stockNuevo = request.Cantidad;
                        break;
                    default:
                        return BadRequest(new { message = "Tipo de movimiento no válido" });
                }

                // Actualizar el stock del insumo
                insumo.StockActual = stockNuevo;
                insumo.FechaActualizacion = DateTime.UtcNow;

                // Si se proporciona un nuevo costo unitario, actualizarlo
                if (request.CostoUnitario.HasValue && request.CostoUnitario.Value > 0)
                {
                    insumo.CostoUnitario = request.CostoUnitario.Value;
                }

                // Crear registro de movimiento de stock
                var movimiento = new MovimientoStock
                {
                    InsumoId = id,
                    TipoMovimiento = request.TipoMovimiento,
                    Cantidad = request.Cantidad,
                    CostoUnitario = request.CostoUnitario ?? insumo.CostoUnitario ?? 0,
                    Motivo = request.Motivo,
                    Documento = request.Documento ?? string.Empty,
                    Fecha = DateTime.UtcNow,
                    StockAnterior = stockAnterior,
                    StockNuevo = stockNuevo
                };

                _context.MovimientosStock.Add(movimiento);
                await _context.SaveChangesAsync();

                return Ok(new { 
                    message = "Stock ajustado correctamente",
                    stockAnterior = stockAnterior,
                    stockNuevo = stockNuevo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al ajustar el stock", error = ex.Message });
            }
        }

        // GET: api/insumos/{id}/movimientos
        [HttpGet("{id}/movimientos")]
        public async Task<ActionResult<IEnumerable<MovimientoStock>>> ObtenerMovimientosStock(int id)
        {
            try
            {
                var insumo = await _context.Insumos.FindAsync(id);
                if (insumo == null)
                {
                    return NotFound(new { message = "Insumo no encontrado" });
                }

                var movimientos = await _context.MovimientosStock
                    .Where(m => m.InsumoId == id)
                    .OrderByDescending(m => m.Fecha)
                    .ToListAsync();

                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}
