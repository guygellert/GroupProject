using Microsoft.EntityFrameworkCore.Migrations;

namespace Caveret.Migrations
{
    public partial class initStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Catagories_catagoryId",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "productName",
                table: "Products",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "catagoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.id);
                    table.ForeignKey(
                        name: "FK_Stock_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stock_productId",
                table: "Stock",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Catagories_catagoryId",
                table: "Products",
                column: "catagoryId",
                principalTable: "Catagories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Catagories_catagoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.AlterColumn<string>(
                name: "productName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "catagoryId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Catagories_catagoryId",
                table: "Products",
                column: "catagoryId",
                principalTable: "Catagories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
