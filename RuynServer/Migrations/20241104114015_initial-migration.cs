using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RuynServer.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LevelData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<string>(type: "TEXT", nullable: true),
                    LevelPackName = table.Column<string>(type: "TINYTEXT", maxLength: 50, nullable: false),
                    Author = table.Column<string>(type: "TINYTEXT", maxLength: 50, nullable: false),
                    LevelCount = table.Column<int>(type: "TINYINT", nullable: false),
                    DownloadCount = table.Column<int>(type: "INT", nullable: false),
                    FileData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Rank = table.Column<int>(type: "INT", nullable: false),
                    FileDataHash = table.Column<string>(type: "TEXT", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoteJuntion",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "TEXT", nullable: false),
                    LevelDataId = table.Column<int>(type: "INTEGER", nullable: false),
                    VoteID = table.Column<int>(type: "INTEGER", nullable: false),
                    VotesVoteId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteJuntion", x => new { x.ClientId, x.LevelDataId });
                    table.ForeignKey(
                        name: "FK_VoteJuntion_LevelData_LevelDataId",
                        column: x => x.LevelDataId,
                        principalTable: "LevelData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoteJuntion_Votes_VotesVoteId",
                        column: x => x.VotesVoteId,
                        principalTable: "Votes",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Votes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "None" },
                    { 1, "Upvote" },
                    { 2, "Downvote" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LevelData_FileDataHash",
                table: "LevelData",
                column: "FileDataHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LevelData_LevelPackName",
                table: "LevelData",
                column: "LevelPackName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoteJuntion_ClientId_LevelDataId",
                table: "VoteJuntion",
                columns: new[] { "ClientId", "LevelDataId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoteJuntion_LevelDataId",
                table: "VoteJuntion",
                column: "LevelDataId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteJuntion_VotesVoteId",
                table: "VoteJuntion",
                column: "VotesVoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoteJuntion");

            migrationBuilder.DropTable(
                name: "LevelData");

            migrationBuilder.DropTable(
                name: "Votes");
        }
    }
}
