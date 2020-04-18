using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class ordernoaddedinextratypeandextra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "DishExtraTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "DishExtras",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "DishExtraTypes");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "DishExtras");
        }
    }
}
