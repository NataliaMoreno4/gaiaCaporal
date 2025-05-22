using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaBlazor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnaTipoUsuarioSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoUsuario",
                schema: "Seg",
                table: "Usuario",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoUsuario",
                schema: "Seg",
                table: "Session",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.InsertData(
                schema: "Seg",
                table: "Rol",
                columns: new[] { "Id", "FechaAdicion", "FechaUltimaActualizacion", "IdUsuarioAdiciono", "IdUsuarioUltimaActualizacion", "IsActive", "Nombre" },
                values: new object[] { 100L, new DateTime(2024, 9, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, null, true, "Default" });

            migrationBuilder.UpdateData(
                schema: "Seg",
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1L,
                column: "TipoUsuario",
                value: "Normal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Seg",
                table: "Rol",
                keyColumn: "Id",
                keyValue: 100L);

            migrationBuilder.DropColumn(
                name: "TipoUsuario",
                schema: "Seg",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "TipoUsuario",
                schema: "Seg",
                table: "Session");
        }
    }
}
