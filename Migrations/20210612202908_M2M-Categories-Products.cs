using Microsoft.EntityFrameworkCore.Migrations;

namespace Caveret.Migrations
{
    public partial class M2MCategoriesProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CatagoriesProducts",
                columns: table => new
                {
                    CatagoriesId = table.Column<int>(type: "int", nullable: false),
                    productsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatagoriesProducts", x => new { x.CatagoriesId, x.productsId });
                    table.ForeignKey(
                        name: "FK_CatagoriesProducts_Catagories_CatagoriesId",
                        column: x => x.CatagoriesId,
                        principalTable: "Catagories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatagoriesProducts_Products_productsId",
                        column: x => x.productsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatagoriesProducts_productsId",
                table: "CatagoriesProducts",
                column: "productsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatagoriesProducts");

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
    }
}
