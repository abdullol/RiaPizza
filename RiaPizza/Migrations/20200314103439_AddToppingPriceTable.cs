using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class AddToppingPriceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SizeToppingPrices",
                columns: table => new
                {
                    SizeToppingPriceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishSizeId = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SizeToppingPrices", x => x.SizeToppingPriceId);
                    table.ForeignKey(
                        name: "FK_SizeToppingPrices_DishSize_DishSizeId",
                        column: x => x.DishSizeId,
                        principalTable: "DishSize",
                        principalColumn: "DishSizeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SizeToppingPrices_DishSizeId",
                table: "SizeToppingPrices",
                column: "DishSizeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SizeToppingPrices");
        }
    }
}
