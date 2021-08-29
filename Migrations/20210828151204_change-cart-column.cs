using Microsoft.EntityFrameworkCore.Migrations;

namespace Caveret.Migrations
{
    public partial class changecartcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopCartItem_AspNetUsers_userId1",
                table: "ShopCartItem");

            migrationBuilder.DropIndex(
                name: "IX_ShopCartItem_userId1",
                table: "ShopCartItem");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "ShopCartItem");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "ShopCartItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "ShopCartItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userId1",
                table: "ShopCartItem",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopCartItem_userId1",
                table: "ShopCartItem",
                column: "userId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCartItem_AspNetUsers_userId1",
                table: "ShopCartItem",
                column: "userId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
