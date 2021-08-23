using Microsoft.EntityFrameworkCore.Migrations;

namespace Caveret.Migrations
{
    public partial class UpdateProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Catagories_catagoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_catagoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "catagoryId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductsId",
                table: "Catagories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catagories_ProductsId",
                table: "Catagories",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catagories_Products_ProductsId",
                table: "Catagories",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catagories_Products_ProductsId",
                table: "Catagories");

            migrationBuilder.DropIndex(
                name: "IX_Catagories_ProductsId",
                table: "Catagories");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "Catagories");

            migrationBuilder.AddColumn<int>(
                name: "catagoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_catagoryId",
                table: "Products",
                column: "catagoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Catagories_catagoryId",
                table: "Products",
                column: "catagoryId",
                principalTable: "Catagories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
