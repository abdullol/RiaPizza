using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class addedplaceholderinsize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Placeholder",
                table: "DishSize",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Placeholder",
                table: "DishSize");
        }
    }
}
