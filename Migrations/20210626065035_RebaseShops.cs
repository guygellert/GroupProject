using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Caveret.Migrations
{
    public partial class RebaseShops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //            migrationBuilder.CreateTable(
            //        name: "Order",
            //        columns: table => new
            //            {
            //            id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //            whenToDeliever = table.Column<DateTime>(type: "datetime2", nullable: false)
            //        },
            //        constraints: table =>
            //            {
            //            table.PrimaryKey("PK_Order", x => x.id);
            //        });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OpeningTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                });

            //  migrationBuilder.CreateTable(
            //      name: "OrderProducts",
            //columns: table => new
            //{
            //    ordersid = table.Column<int>(type: "int", nullable: false),
            //        productsId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_OrderProducts", x => new { x.ordersid, x.productsId });
            //        table.ForeignKey(
            //            name: "FK_OrderProducts_Order_ordersid",
            //            column: x => x.ordersid,
            //            principalTable: "Order",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_OrderProducts_Products_productsId",
            //            column: x => x.productsId,
            //            principalTable: "Products",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });
            //
            //migrationBuilder.CreateIndex(
            //    name: "IX_OrderProducts_productsId",
            //    table: "OrderProducts",
            //    column: "productsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
