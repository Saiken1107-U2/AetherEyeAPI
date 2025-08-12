using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AetherEyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposVentaCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Agregar nuevas columnas para información del cliente
            migrationBuilder.AddColumn<string>(
                name: "NombreCliente",
                table: "Ventas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorreoCliente",
                table: "Ventas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelefonoCliente",
                table: "Ventas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "MetodoPago",
                table: "Ventas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Ventas",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroFactura",
                table: "Ventas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "Ventas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Impuestos",
                table: "Ventas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "Ventas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            // Crear índices para mejorar el rendimiento
            migrationBuilder.CreateIndex(
                name: "IX_Ventas_Estado",
                table: "Ventas",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_NumeroFactura",
                table: "Ventas",
                column: "NumeroFactura",
                unique: true,
                filter: "[NumeroFactura] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_NombreCliente",
                table: "Ventas",
                column: "NombreCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar índices
            migrationBuilder.DropIndex(
                name: "IX_Ventas_Estado",
                table: "Ventas");

            migrationBuilder.DropIndex(
                name: "IX_Ventas_NumeroFactura",
                table: "Ventas");

            migrationBuilder.DropIndex(
                name: "IX_Ventas_NombreCliente",
                table: "Ventas");

            // Eliminar columnas
            migrationBuilder.DropColumn(
                name: "NombreCliente",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "CorreoCliente",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "TelefonoCliente",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "DireccionCliente",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "NumeroFactura",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Impuestos",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "Ventas");
        }
    }
}
