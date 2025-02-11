using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animalia.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultasYVeterinariosFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "veterinarios",
                columns: table => new
                {
                    IdVeterinario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_veterinarios", x => x.IdVeterinario);
                });

            migrationBuilder.CreateTable(
                name: "consultas",
                columns: table => new
                {
                    IdConsulta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMascota = table.Column<int>(type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdVeterinario = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    VeterinarioIdVeterinario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consultas", x => x.IdConsulta);
                    table.ForeignKey(
                        name: "FK_consultas_clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_consultas_mascotas_IdMascota",
                        column: x => x.IdMascota,
                        principalTable: "mascotas",
                        principalColumn: "IdMascota",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_consultas_veterinarios_IdVeterinario",
                        column: x => x.IdVeterinario,
                        principalTable: "veterinarios",
                        principalColumn: "IdVeterinario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_consultas_veterinarios_VeterinarioIdVeterinario",
                        column: x => x.VeterinarioIdVeterinario,
                        principalTable: "veterinarios",
                        principalColumn: "IdVeterinario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_consultas_IdCliente",
                table: "consultas",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_consultas_IdMascota",
                table: "consultas",
                column: "IdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_consultas_IdVeterinario",
                table: "consultas",
                column: "IdVeterinario");

            migrationBuilder.CreateIndex(
                name: "IX_consultas_VeterinarioIdVeterinario",
                table: "consultas",
                column: "VeterinarioIdVeterinario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "consultas");

            migrationBuilder.DropTable(
                name: "veterinarios");
        }
    }
}
