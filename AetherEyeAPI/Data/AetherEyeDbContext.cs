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
        public DbSet<MovimientoStock> MovimientosStock { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para Insumo
            modelBuilder.Entity<Insumo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.CodigoInterno).HasMaxLength(50);
                entity.Property(e => e.Categoria).HasMaxLength(50);
                entity.Property(e => e.UnidadMedida).HasMaxLength(20);
                entity.Property(e => e.CostoUnitario).HasColumnType("decimal(18,2)");
                entity.Property(e => e.StockActual).HasColumnType("decimal(18,2)");
                entity.Property(e => e.StockMinimo).HasColumnType("decimal(18,2)");
                entity.Property(e => e.StockMaximo).HasColumnType("decimal(18,2)");
                
                // Mapear la propiedad FechaActualizacion a la columna FechaUltimaActualizacion
                entity.Property(e => e.FechaActualizacion).HasColumnName("FechaUltimaActualizacion");

                // Relación con Proveedor
                entity.HasOne(e => e.Proveedor)
                      .WithMany()
                      .HasForeignKey(e => e.ProveedorId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configuración para Compra
            modelBuilder.Entity<Compra>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Estado).HasMaxLength(20);
                entity.Property(e => e.NumeroFactura).HasMaxLength(50);
                entity.Property(e => e.Observaciones).HasMaxLength(1000);

                // Relación con Proveedor
                entity.HasOne(e => e.Proveedor)
                      .WithMany()
                      .HasForeignKey(e => e.ProveedorId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relación con DetalleCompras
                entity.HasMany(e => e.DetallesCompra)
                      .WithOne(dc => dc.Compra)
                      .HasForeignKey(dc => dc.CompraId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para DetalleCompra
            modelBuilder.Entity<DetalleCompra>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Cantidad).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CostoUnitario).HasColumnType("decimal(18,2)");

                // Relación con Insumo
                entity.HasOne(e => e.Insumo)
                      .WithMany()
                      .HasForeignKey(e => e.InsumoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Receta (Sistema IoT simplificado)
            modelBuilder.Entity<Receta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CantidadNecesaria).IsRequired();
                entity.Property(e => e.UnidadMedida).HasMaxLength(50).IsRequired();
                entity.Property(e => e.FechaCreacion).IsRequired();

                // Relación con Producto principal
                entity.HasOne(e => e.Producto)
                      .WithMany()
                      .HasForeignKey(e => e.ProductoId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación con Insumo (requerido)
                entity.HasOne(e => e.Insumo)
                      .WithMany()
                      .HasForeignKey(e => e.InsumoId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Índice para evitar duplicados
                entity.HasIndex(e => new { e.ProductoId, e.InsumoId }).IsUnique();
            });

            // Configuración para Venta
            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Subtotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Impuestos).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Descuento).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Estado).HasMaxLength(50).HasDefaultValue("Pendiente");
                entity.Property(e => e.NombreCliente).HasMaxLength(100);
                entity.Property(e => e.CorreoCliente).HasMaxLength(100);
                entity.Property(e => e.TelefonoCliente).HasMaxLength(20);
                entity.Property(e => e.DireccionCliente).HasMaxLength(200);
                entity.Property(e => e.MetodoPago).HasMaxLength(50);
                entity.Property(e => e.NumeroFactura).HasMaxLength(50);
                entity.Property(e => e.Observaciones).HasMaxLength(1000);

                // Relación con Usuario
                entity.HasOne(e => e.Usuario)
                      .WithMany()
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relación con DetalleVentas
                entity.HasMany(e => e.DetallesVenta)
                      .WithOne(dv => dv.Venta)
                      .HasForeignKey(dv => dv.VentaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para DetalleVenta
            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Cantidad).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18,2)");

                // Relación con Producto
                entity.HasOne(e => e.Producto)
                      .WithMany()
                      .HasForeignKey(e => e.ProductoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Comentario
            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ComentarioTexto).HasMaxLength(1000).IsRequired();
                entity.Property(e => e.EstadoSeguimiento).HasMaxLength(50).HasDefaultValue("Pendiente");
                entity.Property(e => e.RespuestaAdmin).HasMaxLength(2000);
                entity.Property(e => e.NotasInternas).HasMaxLength(1000);
                entity.Property(e => e.CategoriaProblema).HasMaxLength(100);
                entity.Property(e => e.Prioridad).HasDefaultValue(2);
                entity.Property(e => e.RequiereAccion).HasDefaultValue(false);

                // Relación con Usuario (cliente)
                entity.HasOne(e => e.Usuario)
                      .WithMany()
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relación con Producto
                entity.HasOne(e => e.Producto)
                      .WithMany()
                      .HasForeignKey(e => e.ProductoId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relación con Admin (Usuario que responde)
                entity.HasOne(e => e.Admin)
                      .WithMany()
                      .HasForeignKey(e => e.AdminId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
