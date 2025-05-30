using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaBlazor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                schema: "Ope",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StockDisponible",
                schema: "Ope",
                table: "Producto",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoUrl",
                schema: "Ope",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "StockDisponible",
                schema: "Ope",
                table: "Producto");
        }
    }
}
