using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class text_2_customTheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DishImageFile",
                table: "CustomizeTheme",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DishImageFile",
                table: "CustomizeTheme");

            migrationBuilder.AddColumn<string>(
                name: "DishImage",
                table: "CustomizeTheme",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
