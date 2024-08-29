using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HfyClientApi.Migrations
{
  /// <inheritdoc />
  public partial class ProcessedAtColumn : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.RenameColumn(
          name: "Edited",
          table: "Chapters",
          newName: "EditedAtUtc");

      migrationBuilder.RenameColumn(
          name: "Created",
          table: "Chapters",
          newName: "CreatedAtUtc");

      migrationBuilder.AddColumn<DateTime>(
          name: "ProcessedAtUtc",
          table: "Chapters",
          type: "timestamp with time zone",
          nullable: false,
          defaultValue: DateTime.UtcNow);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "ProcessedAtUtc",
          table: "Chapters");

      migrationBuilder.RenameColumn(
          name: "EditedAtUtc",
          table: "Chapters",
          newName: "Edited");

      migrationBuilder.RenameColumn(
          name: "CreatedAtUtc",
          table: "Chapters",
          newName: "Created");
    }
  }
}
