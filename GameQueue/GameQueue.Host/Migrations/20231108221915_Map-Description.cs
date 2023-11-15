using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameQueue.Host.Migrations
{
    /// <inheritdoc />
    public partial class MapDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "maps",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "maps");
        }
    }
}
