using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaBlazor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDefaultPasswordAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Seg",
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Celular", "Clave" },
                values: new object[] { "3174575592", "B9A465912169BEF97138C76EFDFD5BB34FDC5FA58855AC187817AE07E80ABE5E-5929B1B6239B2767DDEDDABC98823ADF" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Seg",
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Celular", "Clave" },
                values: new object[] { "", "$2a$10$shiRv6MY8eRdGrMd./ISYOSdhkxcfEEulDJQeVzF8JTGUJi/jK1Pq" });
        }
    }
}
