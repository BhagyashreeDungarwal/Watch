using Microsoft.EntityFrameworkCore.Migrations;
using Watch.Models;

#nullable disable

namespace Watch.Migrations
{
    /// <inheritdoc />
    public partial class AddShopingCartToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    //ApplicationUserId =table.Column<string>(type: "nvarchar(450"),
                },
                constraints: table =>
                {
                    //table.ForeignKey(
                    //    name: "FK_ShopingCarts_AspNetUsers_ApplicationUserId",
                    //    column: x => x.ApplicationUserId,
                    //    principalTable: "AspNetUsers",
                    //    principalColumn: "Id",
                    //    onDelete: ReferentialAction.Cascade);

                    table.PrimaryKey("PK_ShopingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopingCarts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopingCarts_ProductId",
                table: "ShopingCarts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ShopingCarts_AspNetUsers_ApplicationUserId",
            //    table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ShopingCarts");
        }
    }
}
