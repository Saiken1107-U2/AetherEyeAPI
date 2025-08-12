using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AetherEyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class CorregirTiposDatosVentas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Corregir tipos de datos para columnas decimales en Ventas
            migrationBuilder.Sql(@"
                -- Verificar y corregir Subtotal
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Subtotal' AND system_type_id != 106)
                BEGIN
                    ALTER TABLE [Ventas] ALTER COLUMN [Subtotal] decimal(18,2) NOT NULL
                END
            ");

            migrationBuilder.Sql(@"
                -- Verificar y corregir Impuestos
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Impuestos' AND system_type_id != 106)
                BEGIN
                    ALTER TABLE [Ventas] ALTER COLUMN [Impuestos] decimal(18,2) NOT NULL
                END
            ");

            migrationBuilder.Sql(@"
                -- Verificar y corregir Descuento
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Descuento' AND system_type_id != 106)
                BEGIN
                    ALTER TABLE [Ventas] ALTER COLUMN [Descuento] decimal(18,2) NOT NULL
                END
            ");

            // También corregir DetalleVentas si es necesario
            migrationBuilder.Sql(@"
                -- Verificar y corregir Cantidad en DetalleVentas
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DetalleVentas]') AND name = 'Cantidad' AND system_type_id != 106)
                BEGIN
                    ALTER TABLE [DetalleVentas] ALTER COLUMN [Cantidad] decimal(18,2) NOT NULL
                END
            ");

            migrationBuilder.Sql(@"
                -- Verificar y corregir PrecioUnitario en DetalleVentas
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DetalleVentas]') AND name = 'PrecioUnitario' AND system_type_id != 106)
                BEGIN
                    ALTER TABLE [DetalleVentas] ALTER COLUMN [PrecioUnitario] decimal(18,2) NOT NULL
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No es necesario hacer rollback ya que estamos corrigiendo tipos de datos
        }
    }
}
