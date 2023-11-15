using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameQueue.Host.Migrations
{
    /// <inheritdoc />
    public partial class ComposeAndDoneDatesForRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "compose_date",
                table: "search_maps_requests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "done_date",
                table: "search_maps_requests",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "compose_date",
                table: "search_maps_requests");

            migrationBuilder.DropColumn(
                name: "done_date",
                table: "search_maps_requests");
        }
    }
}
