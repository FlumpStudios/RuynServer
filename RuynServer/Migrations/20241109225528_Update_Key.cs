using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuynServer.Migrations
{
    /// <inheritdoc />
    public partial class Update_Key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VoteJuntion_ClientId_LevelPackName",
                table: "VoteJuntion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leaderboard",
                table: "Leaderboard");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leaderboard",
                table: "Leaderboard",
                columns: new[] { "ClientId", "LevelPackName", "LevelNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Leaderboard",
                table: "Leaderboard");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leaderboard",
                table: "Leaderboard",
                columns: new[] { "LevelPackName", "LevelNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_VoteJuntion_ClientId_LevelPackName",
                table: "VoteJuntion",
                columns: new[] { "ClientId", "LevelPackName" },
                unique: true);
        }
    }
}
