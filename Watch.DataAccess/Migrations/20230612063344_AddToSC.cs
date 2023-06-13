using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watch.Migrations
{
    /// <inheritdoc />
    public partial class AddToSC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ShopingCarts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ShopingCarts_ApplicationUserId",
                table: "ShopingCarts",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopingCarts_AspNetUsers_ApplicationUserId",
                table: "ShopingCarts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopingCarts_AspNetUsers_ApplicationUserId",
                table: "ShopingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShopingCarts_ApplicationUserId",
                table: "ShopingCarts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ShopingCarts");
        }
    }
}
