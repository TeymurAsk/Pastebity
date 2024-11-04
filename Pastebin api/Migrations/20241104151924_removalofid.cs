using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pastebin_api.Migrations
{
    /// <inheritdoc />
    public partial class removalofid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TextBlocks",
                table: "TextBlocks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TextBlocks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextBlocks",
                table: "TextBlocks",
                column: "Link");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TextBlocks",
                table: "TextBlocks");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TextBlocks",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextBlocks",
                table: "TextBlocks",
                column: "Id");
        }
    }
}
