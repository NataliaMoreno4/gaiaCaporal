using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaBlazor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGaiaCaporal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Ope");

            migrationBuilder.CreateTable(
                name: "Carrito",
                schema: "Ope",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carrito", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetallePedido",
                schema: "Ope",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<long>(type: "bigint", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallePedido", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                schema: "Ope",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IdCategoria = table.Column<long>(type: "bigint", nullable: false),
                    CategoriaId = table.Column<long>(type: "bigint", nullable: true),
                    IdMercado = table.Column<long>(type: "bigint", nullable: false),
                    MercadoId = table.Column<long>(type: "bigint", nullable: true),
                    CarritoId = table.Column<long>(type: "bigint", nullable: true),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producto_Carrito_CarritoId",
                        column: x => x.CarritoId,
                        principalSchema: "Ope",
                        principalTable: "Carrito",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Producto_ParametroDetallado_CategoriaId",
                        column: x => x.CategoriaId,
                        principalSchema: "Par",
                        principalTable: "ParametroDetallado",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Producto_Usuario_MercadoId",
                        column: x => x.MercadoId,
                        principalSchema: "Seg",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "Seg",
                table: "Modulo",
                columns: new[] { "Id", "FechaAdicion", "FechaUltimaActualizacion", "IdUsuarioAdiciono", "IdUsuarioUltimaActualizacion", "Nivel", "NombreModulo", "TipoModulo" },
                values: new object[] { 7L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, null, "2", "Productos", "Módulo" });

            migrationBuilder.InsertData(
                schema: "Par",
                table: "ParametroGeneral",
                columns: new[] { "Id", "FechaAdicion", "FechaUltimaActualizacion", "IdUsuarioAdiciono", "IdUsuarioUltimaActualizacion", "Nombre" },
                values: new object[] { 2L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, null, "CategoriaProducto" });

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CarritoId",
                schema: "Ope",
                table: "Producto",
                column: "CarritoId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CategoriaId",
                schema: "Ope",
                table: "Producto",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_MercadoId",
                schema: "Ope",
                table: "Producto",
                column: "MercadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallePedido",
                schema: "Ope");

            migrationBuilder.DropTable(
                name: "Producto",
                schema: "Ope");

            migrationBuilder.DropTable(
                name: "Carrito",
                schema: "Ope");

            migrationBuilder.DeleteData(
                schema: "Seg",
                table: "Modulo",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                schema: "Par",
                table: "ParametroGeneral",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
