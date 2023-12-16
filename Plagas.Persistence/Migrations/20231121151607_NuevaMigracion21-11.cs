using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plagas.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NuevaMigracion2111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Plagas");

            migrationBuilder.CreateTable(
                name: "Tipos",
                schema: "Plagas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trampas",
                schema: "Plagas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TiposId = table.Column<int>(type: "int", nullable: false),
                    FechaInstalacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    ImageUrl = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Responsable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trampas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trampas_Tipos_TiposId",
                        column: x => x.TiposId,
                        principalSchema: "Plagas",
                        principalTable: "Tipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trampas_Nombre",
                schema: "Plagas",
                table: "Trampas",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Trampas_TiposId",
                schema: "Plagas",
                table: "Trampas",
                column: "TiposId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trampas",
                schema: "Plagas");

            migrationBuilder.DropTable(
                name: "Tipos",
                schema: "Plagas");
        }
    }
}
