using AetherEyeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AetherEyeAPI.Data
{
    public class AetherEyeDbContext : DbContext
    {
        public AetherEyeDbContext(DbContextOptions<AetherEyeDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }
        public DbSet<Cotizacion> Cotizaciones { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Faq> Faqs { get; set; }
    }
}
