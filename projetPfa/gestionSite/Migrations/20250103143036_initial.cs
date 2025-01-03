using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionSite.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Terrains_TerrainStatuses__TerrainStatusId",
                table: "Terrains");

            migrationBuilder.DropIndex(
                name: "IX_Terrains__TerrainStatusId",
                table: "Terrains");

            migrationBuilder.DropColumn(
                name: "_TerrainStatusId",
                table: "Terrains");

            migrationBuilder.AddColumn<int>(
                name: "TerrainStatusId",
                table: "Terrains",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TerrainStatusId",
                table: "Terrains");

            migrationBuilder.AddColumn<int>(
                name: "_TerrainStatusId",
                table: "Terrains",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Terrains__TerrainStatusId",
                table: "Terrains",
                column: "_TerrainStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Terrains_TerrainStatuses__TerrainStatusId",
                table: "Terrains",
                column: "_TerrainStatusId",
                principalTable: "TerrainStatuses",
                principalColumn: "Id");
        }
    }
}
