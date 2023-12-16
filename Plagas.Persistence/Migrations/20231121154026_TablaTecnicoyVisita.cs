using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plagas.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TablaTecnicoyVisita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tecnicos",
                schema: "Plagas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tecnicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Visita",
                schema: "Plagas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TecnicoId = table.Column<int>(type: "int", nullable: false),
                    TrampaId = table.Column<int>(type: "int", nullable: false),
                    FechaVisita = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "GETDATE()"),
                    OperationNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visita", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visita_Tecnicos_TecnicoId",
                        column: x => x.TecnicoId,
                        principalSchema: "Plagas",
                        principalTable: "Tecnicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visita_Trampas_TrampaId",
                        column: x => x.TrampaId,
                        principalSchema: "Plagas",
                        principalTable: "Trampas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visita_TecnicoId",
                schema: "Plagas",
                table: "Visita",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Visita_TrampaId",
                schema: "Plagas",
                table: "Visita",
                column: "TrampaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visita",
                schema: "Plagas");

            migrationBuilder.DropTable(
                name: "Tecnicos",
                schema: "Plagas");
        }
    }
}
