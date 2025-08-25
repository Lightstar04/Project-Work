using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class app_user_inventories_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "InventoryAccesses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAccesses_AppUserId",
                table: "InventoryAccesses",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryAccesses_AspNetUsers_AppUserId",
                table: "InventoryAccesses",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryAccesses_AspNetUsers_AppUserId",
                table: "InventoryAccesses");

            migrationBuilder.DropIndex(
                name: "IX_InventoryAccesses_AppUserId",
                table: "InventoryAccesses");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "InventoryAccesses");
        }
    }
}
