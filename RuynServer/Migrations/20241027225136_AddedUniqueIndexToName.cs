using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuynServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueIndexToName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LevelData_LevelPackName",
                table: "LevelData",
                column: "LevelPackName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LevelData_LevelPackName",
                table: "LevelData");
        }
    }
}
