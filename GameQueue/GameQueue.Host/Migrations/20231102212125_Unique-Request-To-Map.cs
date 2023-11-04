using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameQueue.Host.Migrations
{
    /// <inheritdoc />
    public partial class UniqueRequestToMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_request_to_maps_search_maps_request_id",
                table: "request_to_maps");

            migrationBuilder.AddUniqueConstraint(
                name: "ak_request_to_maps_search_maps_request_id_map_id",
                table: "request_to_maps",
                columns: new[] { "search_maps_request_id", "map_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "ak_request_to_maps_search_maps_request_id_map_id",
                table: "request_to_maps");

            migrationBuilder.CreateIndex(
                name: "ix_request_to_maps_search_maps_request_id",
                table: "request_to_maps",
                column: "search_maps_request_id");
        }
    }
}
