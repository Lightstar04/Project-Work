using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class unnecessary_migrations_removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryAccesses_AspNetUsers_UserId",
                table: "InventoryAccesses");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryAccesses_AspNetUsers_UserId",
                table: "InventoryAccesses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryAccesses_AspNetUsers_UserId",
                table: "InventoryAccesses");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryAccesses_AspNetUsers_UserId",
                table: "InventoryAccesses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
