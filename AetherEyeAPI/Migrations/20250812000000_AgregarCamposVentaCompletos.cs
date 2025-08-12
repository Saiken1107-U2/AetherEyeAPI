using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AetherEyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposVentaCompletos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Agregar columnas de información del cliente a Ventas
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'NombreCliente')
                BEGIN
                    ALTER TABLE [Ventas] ADD [NombreCliente] nvarchar(100) NULL
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'CorreoCliente')
                BEGIN
                    ALTER TABLE [Ventas] ADD [CorreoCliente] nvarchar(100) NULL
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'TelefonoCliente')
                BEGIN
                    ALTER TABLE [Ventas] ADD [TelefonoCliente] nvarchar(20) NULL
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'DireccionCliente')
                BEGIN
                    ALTER TABLE [Ventas] ADD [DireccionCliente] nvarchar(200) NULL
                END
            ");

            // Agregar columnas de información de la venta
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Estado')
                BEGIN
                    ALTER TABLE [Ventas] ADD [Estado] nvarchar(50) NOT NULL DEFAULT 'Pendiente'
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'MetodoPago')
                BEGIN
                    ALTER TABLE [Ventas] ADD [MetodoPago] nvarchar(50) NULL
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Observaciones')
                BEGIN
                    ALTER TABLE [Ventas] ADD [Observaciones] nvarchar(1000) NULL
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'NumeroFactura')
                BEGIN
                    ALTER TABLE [Ventas] ADD [NumeroFactura] nvarchar(50) NULL
                END
            ");

            // Agregar columnas de cálculos
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Subtotal')
                BEGIN
                    ALTER TABLE [Ventas] ADD [Subtotal] decimal(18,2) NOT NULL DEFAULT 0
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Impuestos')
                BEGIN
                    ALTER TABLE [Ventas] ADD [Impuestos] decimal(18,2) NOT NULL DEFAULT 0
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Descuento')
                BEGIN
                    ALTER TABLE [Ventas] ADD [Descuento] decimal(18,2) NOT NULL DEFAULT 0
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar columnas en caso de rollback
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'NombreCliente') ALTER TABLE [Ventas] DROP COLUMN [NombreCliente]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'CorreoCliente') ALTER TABLE [Ventas] DROP COLUMN [CorreoCliente]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'TelefonoCliente') ALTER TABLE [Ventas] DROP COLUMN [TelefonoCliente]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'DireccionCliente') ALTER TABLE [Ventas] DROP COLUMN [DireccionCliente]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Estado') ALTER TABLE [Ventas] DROP COLUMN [Estado]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'MetodoPago') ALTER TABLE [Ventas] DROP COLUMN [MetodoPago]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Observaciones') ALTER TABLE [Ventas] DROP COLUMN [Observaciones]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'NumeroFactura') ALTER TABLE [Ventas] DROP COLUMN [NumeroFactura]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Subtotal') ALTER TABLE [Ventas] DROP COLUMN [Subtotal]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Impuestos') ALTER TABLE [Ventas] DROP COLUMN [Impuestos]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Descuento') ALTER TABLE [Ventas] DROP COLUMN [Descuento]");
        }
    }
}
