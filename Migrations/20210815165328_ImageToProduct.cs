using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Caveret.Migrations
{
    public partial class ImageToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imgUrl",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "imgUrlId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_imgUrlId",
                table: "Products",
                column: "imgUrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Image_imgUrlId",
                table: "Products",
                column: "imgUrlId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Image_imgUrlId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Products_imgUrlId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "imgUrlId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "imgUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
