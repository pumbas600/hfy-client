using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HfyClientApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFirstChapterFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Chapters_FirstChapterId",
                table: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_Stories_FirstChapterId",
                table: "Stories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stories_FirstChapterId",
                table: "Stories",
                column: "FirstChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Chapters_FirstChapterId",
                table: "Stories",
                column: "FirstChapterId",
                principalTable: "Chapters",
                principalColumn: "Id");
        }
    }
}
