using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AetherEyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposVentaYDetalleVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVentas_Productos_ProductoId",
                table: "DetalleVentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Usuarios_UsuarioId",
                table: "Ventas");

            migrationBuilder.AddColumn<string>(
                name: "CorreoCliente",
                table: "Ventas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "Ventas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DireccionCliente",
                table: "Ventas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Ventas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Pendiente");

            migrationBuilder.AddColumn<decimal>(
                name: "Impuestos",
                table: "Ventas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "MetodoPago",
                table: "Ventas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreCliente",
                table: "Ventas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroFactura",
                table: "Ventas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Ventas",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "Ventas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TelefonoCliente",
                table: "Ventas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cantidad",
                table: "DetalleVentas",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVentas_Productos_ProductoId",
                table: "DetalleVentas",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Usuarios_UsuarioId",
                table: "Ventas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVentas_Productos_ProductoId",
                table: "DetalleVentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Usuarios_UsuarioId",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "CorreoCliente",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "DireccionCliente",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Impuestos",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "NombreCliente",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "NumeroFactura",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "TelefonoCliente",
                table: "Ventas");

            migrationBuilder.AlterColumn<int>(
                name: "Cantidad",
                table: "DetalleVentas",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVentas_Productos_ProductoId",
                table: "DetalleVentas",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Usuarios_UsuarioId",
                table: "Ventas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
