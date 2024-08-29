using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HfyClientApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNextPrevChapterFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Chapters_NextChapterId",
                table: "Chapters");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_NextChapterId",
                table: "Chapters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Chapters_NextChapterId",
                table: "Chapters",
                column: "NextChapterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Chapters_NextChapterId",
                table: "Chapters",
                column: "NextChapterId",
                principalTable: "Chapters",
                principalColumn: "Id");
        }
    }
}
