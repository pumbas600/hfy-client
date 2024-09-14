using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HfyClientApi.Migrations
{
    /// <inheritdoc />
    public partial class IconColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconBackgroundColor",
                table: "Subreddits",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconBackgroundColor",
                table: "Subreddits");
        }
    }
}
