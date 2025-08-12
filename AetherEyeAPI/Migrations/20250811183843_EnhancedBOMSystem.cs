using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AetherEyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedBOMSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Proveedores_ProveedorId",
                table: "Compras");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleCompras_Insumos_InsumoId",
                table: "DetalleCompras");

            migrationBuilder.DropForeignKey(
                name: "FK_Recetas_Insumos_InsumoId",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_ProductoId",
                table: "Recetas");

            migrationBuilder.AlterColumn<int>(
                name: "InsumoId",
                table: "Recetas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "CantidadNecesaria",
                table: "Recetas",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "EsCritico",
                table: "Recetas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Recetas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Nivel",
                table: "Recetas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Recetas",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrdenSecuencia",
                table: "Recetas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PorcentajeMerma",
                table: "Recetas",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SubproductoId",
                table: "Recetas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TiempoPreparacion",
                table: "Recetas",
                type: "decimal(8,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnidadMedida",
                table: "Recetas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Proveedores",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Proveedores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Proveedores",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Proveedores",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "Proveedores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "Proveedores",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Proveedores",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "Proveedores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Proveedores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NombreContacto",
                table: "Proveedores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "Proveedores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SitioWeb",
                table: "Proveedores",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnidadMedida",
                table: "Insumos",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "StockActual",
                table: "Insumos",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Insumos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaUltimaActualizacion",
                table: "Insumos",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostoUnitario",
                table: "Insumos",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Insumos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoInterno",
                table: "Insumos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Insumos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Insumos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaVencimiento",
                table: "Insumos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProveedorId",
                table: "Insumos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StockMaximo",
                table: "Insumos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StockMinimo",
                table: "Insumos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cantidad",
                table: "DetalleCompras",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Compras",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroFactura",
                table: "Compras",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Compras",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MovimientosStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsumoId = table.Column<int>(type: "int", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StockAnterior = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockNuevo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientosStock_Insumos_InsumoId",
                        column: x => x.InsumoId,
                        principalTable: "Insumos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_Nivel",
                table: "Recetas",
                column: "Nivel");

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_ProductoId_OrdenSecuencia",
                table: "Recetas",
                columns: new[] { "ProductoId", "OrdenSecuencia" });

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_SubproductoId",
                table: "Recetas",
                column: "SubproductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_ProveedorId",
                table: "Insumos",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_InsumoId",
                table: "MovimientosStock",
                column: "InsumoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Proveedores_ProveedorId",
                table: "Compras",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleCompras_Insumos_InsumoId",
                table: "DetalleCompras",
                column: "InsumoId",
                principalTable: "Insumos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Insumos_Proveedores_ProveedorId",
                table: "Insumos",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Recetas_Insumos_InsumoId",
                table: "Recetas",
                column: "InsumoId",
                principalTable: "Insumos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recetas_Productos_SubproductoId",
                table: "Recetas",
                column: "SubproductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Proveedores_ProveedorId",
                table: "Compras");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleCompras_Insumos_InsumoId",
                table: "DetalleCompras");

            migrationBuilder.DropForeignKey(
                name: "FK_Insumos_Proveedores_ProveedorId",
                table: "Insumos");

            migrationBuilder.DropForeignKey(
                name: "FK_Recetas_Insumos_InsumoId",
                table: "Recetas");

            migrationBuilder.DropForeignKey(
                name: "FK_Recetas_Productos_SubproductoId",
                table: "Recetas");

            migrationBuilder.DropTable(
                name: "MovimientosStock");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_Nivel",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_ProductoId_OrdenSecuencia",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_SubproductoId",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_Insumos_ProveedorId",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "EsCritico",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "OrdenSecuencia",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "PorcentajeMerma",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "SubproductoId",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "TiempoPreparacion",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "NombreContacto",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "SitioWeb",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "CodigoInterno",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "FechaVencimiento",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "ProveedorId",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "StockMaximo",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "StockMinimo",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "NumeroFactura",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Compras");

            migrationBuilder.AlterColumn<int>(
                name: "InsumoId",
                table: "Recetas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CantidadNecesaria",
                table: "Recetas",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "UnidadMedida",
                table: "Insumos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StockActual",
                table: "Insumos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Insumos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaUltimaActualizacion",
                table: "Insumos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CostoUnitario",
                table: "Insumos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Cantidad",
                table: "DetalleCompras",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_ProductoId",
                table: "Recetas",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Proveedores_ProveedorId",
                table: "Compras",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleCompras_Insumos_InsumoId",
                table: "DetalleCompras",
                column: "InsumoId",
                principalTable: "Insumos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recetas_Insumos_InsumoId",
                table: "Recetas",
                column: "InsumoId",
                principalTable: "Insumos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
