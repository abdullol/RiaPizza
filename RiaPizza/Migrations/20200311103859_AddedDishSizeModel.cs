using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class AddedDishSizeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "BasePrice",
                table: "Dishes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "DishSize",
                columns: table => new
                {
                    DishSizeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<string>(nullable: true),
                    BasePrice = table.Column<float>(nullable: false),
                    Diameter = table.Column<string>(nullable: true),
                    DishId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishSize", x => x.DishSizeId);
                    table.ForeignKey(
                        name: "FK_DishSize_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "DishId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishSize_DishId",
                table: "DishSize",
                column: "DishId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishSize");

            migrationBuilder.AlterColumn<int>(
                name: "BasePrice",
                table: "Dishes",
                type: "int",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
