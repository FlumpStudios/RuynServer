using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuynServer.Migrations
{
    /// <inheritdoc />
    public partial class MakeLevelDataNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaderboard_LevelData_LevelDataLevelPackName",
                table: "Leaderboard");

            migrationBuilder.AlterColumn<string>(
                name: "LevelDataLevelPackName",
                table: "Leaderboard",
                type: "TINYTEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TINYTEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaderboard_LevelData_LevelDataLevelPackName",
                table: "Leaderboard",
                column: "LevelDataLevelPackName",
                principalTable: "LevelData",
                principalColumn: "LevelPackName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaderboard_LevelData_LevelDataLevelPackName",
                table: "Leaderboard");

            migrationBuilder.AlterColumn<string>(
                name: "LevelDataLevelPackName",
                table: "Leaderboard",
                type: "TINYTEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TINYTEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaderboard_LevelData_LevelDataLevelPackName",
                table: "Leaderboard",
                column: "LevelDataLevelPackName",
                principalTable: "LevelData",
                principalColumn: "LevelPackName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
