using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AetherEyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class SimplificacionRecetasIoT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recetas_Productos_SubproductoId",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_Nivel",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_ProductoId_OrdenSecuencia",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_SubproductoId",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "EsCritico",
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

            migrationBuilder.AlterColumn<string>(
                name: "UnidadMedida",
                table: "Recetas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "InsumoId",
                table: "Recetas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CantidadNecesaria",
                table: "Recetas",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_ProductoId_InsumoId",
                table: "Recetas",
                columns: new[] { "ProductoId", "InsumoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recetas_ProductoId_InsumoId",
                table: "Recetas");

            migrationBuilder.AlterColumn<string>(
                name: "UnidadMedida",
                table: "Recetas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

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
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "EsCritico",
                table: "Recetas",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Recetas_Productos_SubproductoId",
                table: "Recetas",
                column: "SubproductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
