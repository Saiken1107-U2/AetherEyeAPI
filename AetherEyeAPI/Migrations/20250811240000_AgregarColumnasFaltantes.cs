using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AetherEyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarColumnasFaltantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Agregar FechaCreacion a Recetas si no existe
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recetas]') AND name = 'FechaCreacion')
                BEGIN
                    ALTER TABLE [Recetas] ADD [FechaCreacion] datetime2 NOT NULL DEFAULT GETDATE()
                END
            ");

            // Agregar UnidadMedida a Recetas si no existe
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recetas]') AND name = 'UnidadMedida')
                BEGIN
                    ALTER TABLE [Recetas] ADD [UnidadMedida] nvarchar(50) NOT NULL DEFAULT 'unidad'
                END
            ");

            // Agregar FechaCreacion a Insumos si no existe
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Insumos]') AND name = 'FechaCreacion')
                BEGIN
                    ALTER TABLE [Insumos] ADD [FechaCreacion] datetime2 NULL DEFAULT GETDATE()
                END
            ");

            // Agregar UnidadMedida a Insumos si no existe
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Insumos]') AND name = 'UnidadMedida')
                BEGIN
                    ALTER TABLE [Insumos] ADD [UnidadMedida] nvarchar(20) NULL
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Opcional: remover las columnas si es necesario
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recetas]') AND name = 'FechaCreacion') ALTER TABLE [Recetas] DROP COLUMN [FechaCreacion]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recetas]') AND name = 'UnidadMedida') ALTER TABLE [Recetas] DROP COLUMN [UnidadMedida]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Insumos]') AND name = 'FechaCreacion') ALTER TABLE [Insumos] DROP COLUMN [FechaCreacion]");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Insumos]') AND name = 'UnidadMedida') ALTER TABLE [Insumos] DROP COLUMN [UnidadMedida]");
        }
    }
}
