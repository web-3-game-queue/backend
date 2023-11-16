using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameQueue.Host.Migrations
{
    /// <inheritdoc />
    public partial class requesthandeldbyuserid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "handled_by_user_id",
                table: "search_maps_requests",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_search_maps_requests_handled_by_user_id",
                table: "search_maps_requests",
                column: "handled_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_search_maps_requests_users_handled_by_user_id",
                table: "search_maps_requests",
                column: "handled_by_user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_search_maps_requests_users_handled_by_user_id",
                table: "search_maps_requests");

            migrationBuilder.DropIndex(
                name: "ix_search_maps_requests_handled_by_user_id",
                table: "search_maps_requests");

            migrationBuilder.DropColumn(
                name: "handled_by_user_id",
                table: "search_maps_requests");
        }
    }
}
