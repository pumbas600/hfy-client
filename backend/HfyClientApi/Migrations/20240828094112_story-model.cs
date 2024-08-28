using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HfyClientApi.Migrations
{
    /// <inheritdoc />
    public partial class storymodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "Subreddit",
                table: "Chapters");

            migrationBuilder.RenameColumn(
                name: "TextHTML",
                table: "Chapters",
                newName: "TextHtml");

            migrationBuilder.AddColumn<int>(
                name: "StoryId",
                table: "Chapters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Author = table.Column<string>(type: "text", nullable: false),
                    Subreddit = table.Column<string>(type: "text", nullable: false),
                    FirstChapterId = table.Column<string>(type: "text", nullable: false),
                    FirstChapterId1 = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stories_Chapters_FirstChapterId1",
                        column: x => x.FirstChapterId1,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_NextChapterId",
                table: "Chapters",
                column: "NextChapterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_StoryId",
                table: "Chapters",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_FirstChapterId1",
                table: "Stories",
                column: "FirstChapterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Chapters_NextChapterId",
                table: "Chapters",
                column: "NextChapterId",
                principalTable: "Chapters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Stories_StoryId",
                table: "Chapters",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Chapters_NextChapterId",
                table: "Chapters");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Stories_StoryId",
                table: "Chapters");

            migrationBuilder.DropTable(
                name: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_NextChapterId",
                table: "Chapters");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_StoryId",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "StoryId",
                table: "Chapters");

            migrationBuilder.RenameColumn(
                name: "TextHtml",
                table: "Chapters",
                newName: "TextHTML");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Chapters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subreddit",
                table: "Chapters",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
