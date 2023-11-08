using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameQueue.Host.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMapPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "maps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "maps",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
