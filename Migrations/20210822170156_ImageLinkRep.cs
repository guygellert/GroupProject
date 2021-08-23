using Microsoft.EntityFrameworkCore.Migrations;

namespace Caveret.Migrations
{
    public partial class ImageLinkRep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
              name: "imgUrlId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ImageLink",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageLink", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_imgUrlId",
                table: "Products",
                column: "imgUrlId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_userId1",
                table: "Order",
                column: "userId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_userId1",
                table: "Order",
                column: "userId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ImageLink_imgUrlId",
                table: "Products",
                column: "imgUrlId",
                principalTable: "ImageLink",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_userId1",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ImageLink_imgUrlId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ImageLink");

            migrationBuilder.DropIndex(
                name: "IX_Products_imgUrlId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Order_userId1",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "imgUrlId",
                table: "Products");
        }
    }
}
