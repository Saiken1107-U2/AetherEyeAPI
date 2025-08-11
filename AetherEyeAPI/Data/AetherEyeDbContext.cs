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
        }
    }
}
