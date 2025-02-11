using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animalia.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConflictingRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Diagnosticos_IdConsulta",
                table: "Diagnosticos");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosticos_IdConsulta",
                table: "Diagnosticos",
                column: "IdConsulta",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Diagnosticos_IdConsulta",
                table: "Diagnosticos");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosticos_IdConsulta",
                table: "Diagnosticos",
                column: "IdConsulta");
        }
    }
}
