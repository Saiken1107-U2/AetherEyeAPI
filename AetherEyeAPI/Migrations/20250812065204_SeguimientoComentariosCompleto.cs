using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AetherEyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeguimientoComentariosCompleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Productos_ProductoId",
                table: "Comentarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Usuarios_UsuarioId",
                table: "Comentarios");

            migrationBuilder.AlterColumn<string>(
                name: "TelefonoCliente",
                table: "Ventas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Ventas",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeroFactura",
                table: "Ventas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NombreCliente",
                table: "Ventas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetodoPago",
                table: "Ventas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Ventas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Pendiente",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DireccionCliente",
                table: "Ventas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorreoCliente",
                table: "Ventas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ComentarioTexto",
                table: "Comentarios",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Comentarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoriaProblema",
                table: "Comentarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoSeguimiento",
                table: "Comentarios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Pendiente");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRespuesta",
                table: "Comentarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotasInternas",
                table: "Comentarios",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Prioridad",
                table: "Comentarios",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<bool>(
                name: "RequiereAccion",
                table: "Comentarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RespuestaAdmin",
                table: "Comentarios",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_AdminId",
                table: "Comentarios",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Productos_ProductoId",
                table: "Comentarios",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Usuarios_AdminId",
                table: "Comentarios",
                column: "AdminId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Usuarios_UsuarioId",
                table: "Comentarios",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Productos_ProductoId",
                table: "Comentarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Usuarios_AdminId",
                table: "Comentarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Usuarios_UsuarioId",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_AdminId",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "CategoriaProblema",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "EstadoSeguimiento",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "FechaRespuesta",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "NotasInternas",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "Prioridad",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "RequiereAccion",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "RespuestaAdmin",
                table: "Comentarios");

            migrationBuilder.AlterColumn<string>(
                name: "TelefonoCliente",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeroFactura",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NombreCliente",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetodoPago",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Pendiente");

            migrationBuilder.AlterColumn<string>(
                name: "DireccionCliente",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorreoCliente",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ComentarioTexto",
                table: "Comentarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Productos_ProductoId",
                table: "Comentarios",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Usuarios_UsuarioId",
                table: "Comentarios",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
