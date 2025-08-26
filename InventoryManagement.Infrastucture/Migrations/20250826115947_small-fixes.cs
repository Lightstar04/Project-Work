using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class smallfixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldValues_Items_ItemId",
                table: "FieldValues");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldValues_Items_ItemId",
                table: "FieldValues",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldValues_Items_ItemId",
                table: "FieldValues");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldValues_Items_ItemId",
                table: "FieldValues",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
