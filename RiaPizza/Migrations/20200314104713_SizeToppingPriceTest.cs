using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class SizeToppingPriceTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SizeToppingPrices_DishSize_DishSizeId",
                table: "SizeToppingPrices");

            migrationBuilder.DropIndex(
                name: "IX_SizeToppingPrices_DishSizeId",
                table: "SizeToppingPrices");

            migrationBuilder.AddColumn<int>(
                name: "DishExtraId",
                table: "SizeToppingPrices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SizeToppingPrices_DishExtraId",
                table: "SizeToppingPrices",
                column: "DishExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_SizeToppingPrices_DishExtras_DishExtraId",
                table: "SizeToppingPrices",
                column: "DishExtraId",
                principalTable: "DishExtras",
                principalColumn: "DishExtraId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SizeToppingPrices_DishExtras_DishExtraId",
                table: "SizeToppingPrices");

            migrationBuilder.DropIndex(
                name: "IX_SizeToppingPrices_DishExtraId",
                table: "SizeToppingPrices");

            migrationBuilder.DropColumn(
                name: "DishExtraId",
                table: "SizeToppingPrices");

            migrationBuilder.CreateIndex(
                name: "IX_SizeToppingPrices_DishSizeId",
                table: "SizeToppingPrices",
                column: "DishSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SizeToppingPrices_DishSize_DishSizeId",
                table: "SizeToppingPrices",
                column: "DishSizeId",
                principalTable: "DishSize",
                principalColumn: "DishSizeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
