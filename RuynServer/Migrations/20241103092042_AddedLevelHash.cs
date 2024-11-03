using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuynServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedLevelHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileDataHash",
                table: "LevelData",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LevelData_FileDataHash",
                table: "LevelData",
                column: "FileDataHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LevelData_FileDataHash",
                table: "LevelData");

            migrationBuilder.DropColumn(
                name: "FileDataHash",
                table: "LevelData");
        }
    }
}
